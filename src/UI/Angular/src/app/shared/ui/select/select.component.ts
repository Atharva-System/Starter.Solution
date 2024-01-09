import { NgClass } from '@angular/common';
import {
  Component,
  EventEmitter,
  Input,
  Output,
  forwardRef,
} from '@angular/core';
import {
  ControlValueAccessor,
  FormsModule,
  NG_VALUE_ACCESSOR,
} from '@angular/forms';

export interface ISelectItems {
  value: string;
  label: string;
}

@Component({
  selector: 'app-select',
  standalone: true,
  imports: [FormsModule, NgClass],
  providers: [
    {
      provide: NG_VALUE_ACCESSOR,
      useExisting: forwardRef(() => SelectComponent),
      multi: true,
    },
  ],
  templateUrl: './select.component.html',
  styleUrl: './select.component.css',
})
export class SelectComponent implements ControlValueAccessor {
  @Input() id = 'select';
  @Input() isSubmit = false;
  @Input() control: any;
  @Input() disable = false;
  @Input() options: ISelectItems[] = [];
  @Input() defaultOptions: string = 'select';
  @Output() selectedValue: EventEmitter<string> = new EventEmitter<string>();
  value: string = '';

  onChange: any = () => {};
  onTouched: any = () => {};

  onSelectOption(option: any) {
    this.onChange(option.target.value);
    this.onTouched();
    this.selectedValue.emit(option.target.value);
  }

  writeValue(obj: any): void {
    this.value = obj ?? '';
  }

  registerOnChange(fn: any): void {
    this.onChange = fn;
  }

  registerOnTouched(fn: any): void {
    this.onTouched = fn;
  }

  setDisabledState?(isDisabled: boolean): void {
    //this.control.disable({ onlySelf: isDisabled, emitEvent: false });
  }
}
