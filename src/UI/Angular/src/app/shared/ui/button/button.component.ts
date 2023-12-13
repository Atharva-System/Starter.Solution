import { NgClass } from '@angular/common';
import { Component, Input } from '@angular/core';

@Component({
  selector: 'app-button',
  templateUrl: './button.component.html',
  styleUrl: './button.component.css',
  standalone: true,
  imports: [NgClass],
})
export class ButtonComponent {
  @Input() text = '';
  @Input() type = 'button';
  @Input() classes = 'btn-primary';
  @Input() disabled = false;
}
