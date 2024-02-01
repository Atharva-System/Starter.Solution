import { NgClass } from '@angular/common';
import { Component, Input, forwardRef } from '@angular/core';
import {
  ReactiveFormsModule,
  NG_VALUE_ACCESSOR,
  ControlValueAccessor,
} from '@angular/forms';
import { QuillModule } from 'ngx-quill';

@Component({
  selector: 'app-text-editor',
  standalone: true,
  imports: [QuillModule, ReactiveFormsModule, NgClass],
  providers: [
    {
      provide: NG_VALUE_ACCESSOR,
      useExisting: forwardRef(() => TextEditorComponent),
      multi: true,
    },
  ],
  templateUrl: './text-editor.component.html',
  styleUrl: './text-editor.component.css',
})
export class TextEditorComponent implements ControlValueAccessor {
  @Input() id = 'txt';
  @Input() type = 'text';
  @Input() placeholder = '';
  @Input() isSubmit = false;
  @Input() control: any;
  @Input() value: string = '';

  editorOptions = {
    toolbar: [
      [{ header: [1, 2, false] }],
      ['bold', 'italic', 'underline', 'link'],
      [{ list: 'ordered' }, { list: 'bullet' }],
      ['clean'],
    ],
  };

  onChange: any = () => {};
  onTouched: any = () => {};

  writeValue(obj: any): void {
    this.value = obj;
  }
  registerOnChange(fn: any): void {
    this.onChange = fn;
  }
  registerOnTouched(fn: any): void {
    this.onTouched = fn;
  }
  setDisabledState?(isDisabled: boolean): void {
  }
}
