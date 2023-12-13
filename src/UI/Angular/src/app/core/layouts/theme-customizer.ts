import { Component, inject } from '@angular/core';
import { Store } from '@ngrx/store';
import { Router } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { NgScrollbarModule } from 'ngx-scrollbar';
import { NgClass } from '@angular/common';

@Component({
  selector: 'setting',
  templateUrl: './theme-customizer.html',
  standalone: true,
  imports: [NgClass, NgScrollbarModule, FormsModule],
})
export class ThemeCustomizerComponent {
  store: any;
  showCustomizer = false;
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

  reloadRoute() {
    window.location.reload();
    this.showCustomizer = true;
  }
}
