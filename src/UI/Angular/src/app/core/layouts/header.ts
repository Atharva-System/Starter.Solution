import { Component, EventEmitter, OnInit, Output, inject } from '@angular/core';
import { Store } from '@ngrx/store';
import {
  Router,
  NavigationEnd,
  RouterLink,
  RouterLinkActive,
} from '@angular/router';
import { AppService } from '../../shared/services/app.service';
import { animate, style, transition, trigger } from '@angular/animations';
import { DomSanitizer } from '@angular/platform-browser';
import { TranslateService, TranslateModule } from '@ngx-translate/core';
import { MenuModule } from 'headlessui-angular';
import { FormsModule } from '@angular/forms';
import { NgClass } from '@angular/common';
import { AuthenticationService } from '../services/authentication.service';
import { appPaths } from '../../shared/constants/routes';
import { UserService } from '../../modules/user/services/user.service';
import { IUserProfileSignal } from '../../modules/user/models/user-profile.interface';
import { MenuService } from '../services/menu.service';
import { SendReciveNotificationService } from '../services/send-recive-notification.service';

@Component({
  selector: 'header',
  templateUrl: './header.html',
  animations: [
    trigger('toggleAnimation', [
      transition(':enter', [
        style({ opacity: 0, transform: 'scale(0.95)' }),
        animate('100ms ease-out', style({ opacity: 1, transform: 'scale(1)' })),
      ]),
      transition(':leave', [
        animate('75ms', style({ opacity: 0, transform: 'scale(0.95)' })),
      ]),
    ]),
  ],
  standalone: true,
  imports: [
    NgClass,
    RouterLink,
    FormsModule,
    MenuModule,
    RouterLinkActive,
    TranslateModule,
  ],
})
export class HeaderComponent implements OnInit {
  @Output() openChangePasswordPopup = new EventEmitter();
  menuItems: Array<{ label: string; link: string }> = [];
  profileRoute = '/' + appPaths.profile;

  sendReciveNotificationService = inject(SendReciveNotificationService);
  menuService = inject(MenuService);
  appSetting = inject(AppService);
  router = inject(Router);
  translate = inject(TranslateService);
  storeData = inject(Store<any>);
  sanitizer = inject(DomSanitizer);
  authenticationService = inject(AuthenticationService);
  userService = inject(UserService);

  profileFromSignal = this.userService.profileSignal;
  notifications = this.sendReciveNotificationService.notifications;

  constructor() {
    this.initStore();
    var userInfo = this.authenticationService.getUser();
    var profile = {
      email: userInfo?.email,
      name: userInfo?.name,
    } as IUserProfileSignal;
    this.userService.setProfileSignal(profile);
  }

  store: any;
  search = false;

  messages = [
    {
      id: 1,
      image: this.sanitizer.bypassSecurityTrustHtml(
        `<span class="grid place-content-center w-9 h-9 rounded-full bg-success-light dark:bg-success text-success dark:text-success-light"><svg xmlns="http://www.w3.org/2000/svg" class="w-5 h-5" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="1.5" stroke-linecap="round" stroke-linejoin="round"><path d="M12 22s8-4 8-10V5l-8-3-8 3v7c0 6 8 10 8 10z"></path></svg></span>`,
      ),
      title: 'Congratulations!',
      message: 'Your OS has been updated.',
      time: '1hr',
    },
    {
      id: 2,
      image: this.sanitizer.bypassSecurityTrustHtml(
        `<span class="grid place-content-center w-9 h-9 rounded-full bg-info-light dark:bg-info text-info dark:text-info-light"><svg xmlns="http://www.w3.org/2000/svg" class="w-5 h-5" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="1.5" stroke-linecap="round" stroke-linejoin="round"><circle cx="12" cy="12" r="10"></circle><line x1="12" y1="16" x2="12" y2="12"></line><line x1="12" y1="8" x2="12.01" y2="8"></line></svg></span>`,
      ),
      title: 'Did you know?',
      message: 'You can switch between artboards.',
      time: '2hr',
    },
    {
      id: 3,
      image: this.sanitizer.bypassSecurityTrustHtml(
        `<span class="grid place-content-center w-9 h-9 rounded-full bg-danger-light dark:bg-danger text-danger dark:text-danger-light"> <svg xmlns="http://www.w3.org/2000/svg" class="w-5 h-5" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="1.5" stroke-linecap="round" stroke-linejoin="round"><line x1="18" y1="6" x2="6" y2="18"></line><line x1="6" y1="6" x2="18" y2="18"></line></svg></span>`,
      ),
      title: 'Something went wrong!',
      message: 'Send Reposrt',
      time: '2days',
    },
    {
      id: 4,
      image: this.sanitizer.bypassSecurityTrustHtml(
        `<span class="grid place-content-center w-9 h-9 rounded-full bg-warning-light dark:bg-warning text-warning dark:text-warning-light"><svg xmlns="http://www.w3.org/2000/svg" class="w-5 h-5" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="1.5" stroke-linecap="round" stroke-linejoin="round">    <circle cx="12" cy="12" r="10"></circle>    <line x1="12" y1="8" x2="12" y2="12"></line>    <line x1="12" y1="16" x2="12.01" y2="16"></line></svg></span>`,
      ),
      title: 'Warning',
      message: 'Your password strength is low.',
      time: '5days',
    },
  ];

  async initStore() {
    this.storeData
      .select((d) => d.index)
      .subscribe((d) => {
        this.store = d;
      });
  }

  ngOnInit() {
    this.setActiveDropdown();
    this.setMenuItems();
    this.router.events.subscribe((event) => {
      if (event instanceof NavigationEnd) {
        this.setActiveDropdown();
      }
    });
  }

  setMenuItems() {
    this.menuItems = this.menuService.getMenus();
  }

  setActiveDropdown() {
    const selector = document.querySelector(
      'ul.horizontal-menu a[routerLink="' + window.location.pathname + '"]',
    );
    if (selector) {
      selector.classList.add('active');
      const all: any = document.querySelectorAll(
        'ul.horizontal-menu .nav-link.active',
      );
      for (let i = 0; i < all.length; i++) {
        all[0]?.classList.remove('active');
      }
      const ul: any = selector.closest('ul.sub-menu');
      if (ul) {
        let ele: any = ul.closest('li.menu').querySelectorAll('.nav-link');
        if (ele) {
          ele = ele[0];
          setTimeout(() => {
            ele?.classList.add('active');
          });
        }
      }
    }
  }

  removeNotification(value: string) {
    this.sendReciveNotificationService.removeNotification(value);
  }

  removeMessage(value: number) {
    this.messages = this.messages.filter((d) => d.id !== value);
  }

  changeLanguage(item: any) {
    this.translate.use(item.code);
    this.appSetting.toggleLanguage(item);
    if (this.store.locale?.toLowerCase() === 'ae') {
      this.storeData.dispatch({ type: 'toggleRTL', payload: 'rtl' });
    } else {
      this.storeData.dispatch({ type: 'toggleRTL', payload: 'ltr' });
    }
    window.location.reload();
  }

  signOut() {
    this.sendReciveNotificationService.leaveSignalRConnection();
    this.authenticationService.signOut();
  }

  openChangePasswordModal() {
    this.openChangePasswordPopup.emit();
  }
}
