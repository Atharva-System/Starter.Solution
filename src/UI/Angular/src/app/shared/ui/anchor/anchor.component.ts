import { NgClass } from '@angular/common';
import { Component, Input } from '@angular/core';
import { RouterModule } from '@angular/router';

@Component({
  selector: 'app-anchor',
  templateUrl: './anchor.component.html',
  styleUrl: './anchor.component.css',
  standalone: true,
  imports: [NgClass, RouterModule],
})
export class AnchorComponent {
  @Input() text: string = '';
  @Input() classes: string = 'text-primary';
  @Input() routerLink!: string;
}
