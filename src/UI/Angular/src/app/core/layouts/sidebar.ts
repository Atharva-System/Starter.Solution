﻿import { Component, inject } from '@angular/core';
import { Router, RouterLink, RouterLinkActive } from '@angular/router';
import { Store } from '@ngrx/store';
import { TranslateService, TranslateModule } from '@ngx-translate/core';
import { NgScrollbarModule } from 'ngx-scrollbar';
import { NgClass } from '@angular/common';
import { slideDownUp } from '../../shared/services/animations.service';
import { appPaths } from '../../shared/constants/routes';

@Component({
  selector: 'sidebar',
  templateUrl: './sidebar.html',
  animations: [slideDownUp],
  standalone: true,
  imports: [
    NgClass,
    RouterLink,
    NgScrollbarModule,
    RouterLinkActive,
    TranslateModule,
  ],
})
export class SidebarComponent {
  routes = {
    users: appPaths.users,
  };
  active = false;
  store: any;
  activeDropdown: string[] = [];
  parentDropdown: string = '';

  translate = inject(TranslateService);
  storeData = inject(Store<any>);
  router = inject(Router);

  constructor() {
    this.initStore();
  }
  async initStore() {
    this.storeData
      .select((d) => d.index)
      .subscribe((d) => {
        this.store = d;
      });
  }

  ngOnInit() {
    this.setActiveDropdown();
  }

  setActiveDropdown() {
    const selector = document.querySelector(
      '.sidebar ul a[routerLink="' + window.location.pathname + '"]',
    );
    if (selector) {
      selector.classList.add('active');
      const ul: any = selector.closest('ul.sub-menu');
      if (ul) {
        let ele: any =
          ul.closest('li.menu').querySelectorAll('.nav-link') || [];
        if (ele.length) {
          ele = ele[0];
          setTimeout(() => {
            ele.click();
          });
        }
      }
    }
  }

  toggleMobileMenu() {
    if (window.innerWidth < 1024) {
      this.storeData.dispatch({ type: 'toggleSidebar' });
    }
  }

  toggleAccordion(name: string, parent?: string) {
    if (this.activeDropdown.includes(name)) {
      this.activeDropdown = this.activeDropdown.filter((d) => d !== name);
    } else {
      this.activeDropdown.push(name);
    }
  }
}
