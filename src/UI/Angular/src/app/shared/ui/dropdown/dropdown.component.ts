import { trigger, transition, style, animate } from '@angular/animations';
import { Component, EventEmitter, Input, Output } from '@angular/core';
import { MenuModule } from 'headlessui-angular';

export interface IDropdownItems {
  text: string;
  value: string;
  selected?: boolean;
  hide?: boolean;
  disable?: boolean;
  type?: any;
}

@Component({
  selector: 'app-dropdown',
  standalone: true,
  imports: [MenuModule],
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
  templateUrl: './dropdown.component.html',
  styleUrl: './dropdown.component.css',
})
export class DropdownComponent {
  @Input() text = 'Select';
  @Input() cols = [] as IDropdownItems[];
  @Output() selectedValue = new EventEmitter<string>();

  onSelect(item: IDropdownItems) {
    this.cols.forEach((i) => {
      i.selected = false;
    });
    this.text = item.text;
    item.selected = true;
    this.selectedValue.emit(item.value);
  }
}
