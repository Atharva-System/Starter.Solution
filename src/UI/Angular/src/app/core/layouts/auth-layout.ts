import { Component, inject } from '@angular/core';
import { Store } from '@ngrx/store';
import { AppService } from '../../shared/services/app.service';
import { RouterOutlet } from '@angular/router';
import { NgClass } from '@angular/common';

@Component({
  selector: 'app-root',
  templateUrl: './auth-layout.html',
  standalone: true,
  imports: [NgClass, RouterOutlet],
})
export class AuthLayout {
  store: any;
  showTopButton = false;
  service = inject(AppService);
  storeData = inject(Store<any>);

  constructor() {
    this.initStore();
  }
  headerClass = '';
  ngOnInit() {
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

  toggleLoader() {
    this.storeData.dispatch({ type: 'toggleMainLoader', payload: true });
    setTimeout(() => {
      this.storeData.dispatch({ type: 'toggleMainLoader', payload: false });
    }, 500);
  }

  ngOnDestroy() {
    window.removeEventListener('scroll', () => {});
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
