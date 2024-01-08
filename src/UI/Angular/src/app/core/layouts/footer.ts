import { Component } from '@angular/core';

@Component({
  selector: 'footer',
  templateUrl: './footer.html',
  standalone: true,
})
export class FooterComponent {
  currYear: number = new Date().getFullYear();
}
