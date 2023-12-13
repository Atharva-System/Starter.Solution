import { TranslateHttpLoader } from '@ngx-translate/http-loader';
import { HttpClient } from '@angular/common/http';

export function httpTranslateLoader(http: HttpClient) {
  return new TranslateHttpLoader(http);
}
