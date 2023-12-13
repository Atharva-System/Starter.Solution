import { Component, inject } from '@angular/core';
import { Store } from '@ngrx/store';
import { AppService } from '../../shared/services/app.service';
import {
  Router,
  NavigationEnd,
  Event as RouterEvent,
  RouterOutlet,
} from '@angular/router';
import { TranslateService } from '@ngx-translate/core';
import { FooterComponent } from './footer';
import { HeaderComponent } from './header';
import { SidebarComponent } from './sidebar';
import { ThemeCustomizerComponent } from './theme-customizer';
import { NgClass } from '@angular/common';

@Component({
  selector: 'app-root',
  templateUrl: './app-layout.html',
  standalone: true,
  imports: [
    NgClass,
    ThemeCustomizerComponent,
    SidebarComponent,
    HeaderComponent,
    RouterOutlet,
    FooterComponent,
  ],
})
export class AppLayout {
  store: any;
  showTopButton = false;
  storeData = inject(Store<any>);
  service = inject(AppService);
  router = inject(Router);
  constructor(public translate: TranslateService) {
    this.initStore();
  }
  headerClass = '';
  ngOnInit() {
    this.initAnimation();
    this.toggleLoader();
    window.addEventListener('scroll', () => {
      if (
        document.body.scrollTop > 50 ||
        document.documentElement.scrollTop > 50
      ) {
        this.showTopButton = true;
      } else {
        this.showTopButton = false;
      }
    });
  }

  ngOnDestroy() {
    window.removeEventListener('scroll', () => {});
  }

  initAnimation() {
    this.service.changeAnimation();
    this.router.events.subscribe((event) => {
      if (event instanceof NavigationEnd) {
        this.service.changeAnimation();
      }
    });

    const ele: any = document.querySelector('.animation');
    ele.addEventListener('animationend', () => {
      this.service.changeAnimation('remove');
    });
  }

  toggleLoader() {
    this.storeData.dispatch({ type: 'toggleMainLoader', payload: true });
    setTimeout(() => {
      this.storeData.dispatch({ type: 'toggleMainLoader', payload: false });
    }, 500);
  }

  async initStore() {
    this.storeData
      .select((d) => d.index)
      .subscribe((d) => {
        this.store = d;
      });
  }

  goToTop() {
    document.body.scrollTop = 0;
    document.documentElement.scrollTop = 0;
  }
}
