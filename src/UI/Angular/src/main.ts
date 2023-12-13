import { httpTranslateLoader } from './app/app.module';
import { provideAnimations } from '@angular/platform-browser/animations';

import {
  BrowserModule,
  Title,
  bootstrapApplication,
} from '@angular/platform-browser';
import { importProvidersFrom } from '@angular/core';
import { StoreModule } from '@ngrx/store';
import { provideRouter, withInMemoryScrolling } from '@angular/router';
import {
  HttpClient,
  provideHttpClient,
  withInterceptorsFromDi,
} from '@angular/common/http';
import { AppComponent } from './app/app.component';
import { TranslateModule, TranslateLoader } from '@ngx-translate/core';
import { MenuModule } from 'headlessui-angular';
import { NgScrollbarModule } from 'ngx-scrollbar';
import { indexReducer } from './app/shared/store/index.reducer';
import { AppService } from './app/shared/services/app.service';
import { routes } from './app/app.route';

bootstrapApplication(AppComponent, {
  providers: [
    importProvidersFrom(
      BrowserModule,
      MenuModule,
      TranslateModule.forRoot({
        loader: {
          provide: TranslateLoader,
          useFactory: httpTranslateLoader,
          deps: [HttpClient],
        },
      }),
      StoreModule.forRoot({ index: indexReducer }),
      NgScrollbarModule,
    ),
    AppService,
    Title,
    provideRouter(
      routes,
      withInMemoryScrolling({ scrollPositionRestoration: 'enabled' }),
    ),
    provideAnimations(),
    provideHttpClient(withInterceptorsFromDi()),
  ],
}).catch((err) => console.error(err));
