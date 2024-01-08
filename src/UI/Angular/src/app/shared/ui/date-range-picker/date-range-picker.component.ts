import { CommonModule } from '@angular/common';
import {
  Component,
  EventEmitter,
  Input,
  Output,
  forwardRef,
} from '@angular/core';
import {
  ControlValueAccessor,
  FormControl,
  FormsModule,
  NG_VALUE_ACCESSOR,
} from '@angular/forms';
import { FlatpickrModule } from 'angularx-flatpickr';

@Component({
  selector: 'app-date-range-picker',
  standalone: true,
  imports: [CommonModule, FormsModule, FlatpickrModule],
  providers: [
    {
      provide: NG_VALUE_ACCESSOR,
      useExisting: forwardRef(() => DateRangePickerComponent),
      multi: true,
    },
  ],
  templateUrl: './date-range-picker.component.html',
  styleUrl: './date-range-picker.component.css',
})
export class DateRangePickerComponent implements ControlValueAccessor {
  @Input() isSubmit = false;
  @Input() minDate = new Date('1990-01-01');
  @Input() placeholder: string = '';
  @Input() classes: string = '';
  @Input() Control: any;
  @Output() onSelect: EventEmitter<{ from: Date; to: Date } | null> =
    new EventEmitter<{ from: Date; to: Date } | null>();
  @Output() onClear: EventEmitter<void> = new EventEmitter<void>();
  @Input() rangeValue: { from: any; to: any } = {
    from: '',
    to: '',
  };

  onChange: any = () => {};
  onTouched: any = () => {};

  ngOnInit() {}

  selectedDateRange(selectedDates: any) {
    if (selectedDates.length === 2) {
      const startDate = selectedDates[0];
      const endDate = selectedDates[1];

      const startDateYear = startDate.getFullYear();
      const startDateMonth = startDate.getMonth();
      const startDateDay = startDate.getDate();
      const startDateUtcDate = new Date(
        Date.UTC(startDateYear, startDateMonth, startDateDay),
      );

      const endDateYear = endDate.getFullYear();
      const endDateMonth = endDate.getMonth();
      const endDateDay = endDate.getDate();
      const endDateUtcDate = new Date(
        Date.UTC(endDateYear, endDateMonth, endDateDay),
      );

      var range = {
        from: startDateUtcDate,
        to: endDateUtcDate,
      };
      this.onChange(range);
      this.onSelect.emit(range);
    } else {
      this.onSelect.emit(null);
      this.onChange(null);
    }
  }

  writeValue(obj: any): void {
    this.rangeValue = obj;
  }

  registerOnChange(fn: any): void {
    this.onChange = fn;
  }

  registerOnTouched(fn: any): void {
    this.onTouched = fn;
  }

  setDisabledState?(isDisabled: boolean): void {}

  clear() {
    this.rangeValue = {
      from: '',
      to: '',
    };
    if (this.Control && this.Control instanceof FormControl) {
      this.Control.setValue('');
      this.Control.markAsTouched();
      this.Control.updateValueAndValidity();
    }
  }

  clearAndEmit() {
    this.onClear.emit();
    this.clear();
  }
}
