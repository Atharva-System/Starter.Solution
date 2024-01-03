import { Component, ViewChild, inject } from '@angular/core';
import { ModalComponent } from '../../../../shared/ui/modal/modal.component';
import { ButtonComponent } from '../../../../shared/ui/button/button.component';
import { InputComponent } from '../../../../shared/ui/input/input.component';
import {
  AbstractControl,
  FormBuilder,
  FormGroup,
  ReactiveFormsModule,
  ValidatorFn,
  Validators,
} from '@angular/forms';
import { NgClass } from '@angular/common';
import { Router, ActivatedRoute } from '@angular/router';
import {
  FieldValidation,
  Regex,
  queryStringParams,
} from '../../../../shared/constants/constants';
import { authPaths } from '../../../../shared/constants/routes';
import { AlertService } from '../../../../shared/services/alert.service';
import { AuthService } from '../../../auth/services/auth.service';

@Component({
  selector: 'app-change-password-modal',
  standalone: true,
  imports: [
    ReactiveFormsModule,
    ModalComponent,
    ButtonComponent,
    InputComponent,
    NgClass,
  ],
  templateUrl: './change-password-modal.component.html',
  styleUrl: './change-password-modal.component.css',
})
export class ChangePasswordModalComponent {
  @ViewChild('modalComponent') modalComponent!: ModalComponent;
  formChangePassword!: FormGroup;
  isSubmitFormChangePassword = false;

  open() {
    setTimeout(() => {
      this.modalComponent.open();
    }, 10);
  }
  authService = inject(AuthService);
  alertService = inject(AlertService);
  router = inject(Router);
  fb = inject(FormBuilder);

  constructor(private route: ActivatedRoute) {
    this.initForm();
  }

  initForm() {
    this.formChangePassword = this.fb.group(
      {
        currentPassword: ['', Validators.compose([Validators.required])],
        newPassword: [
          '',
          Validators.compose([
            Validators.required,
            Validators.minLength(FieldValidation.passwordMinLength),
            Validators.pattern(Regex.passwordValidationPattern),
          ]),
        ],
        confirmPassword: [
          '',
          Validators.compose([
            Validators.required,
            Validators.minLength(FieldValidation.passwordMinLength),
            Validators.pattern(Regex.passwordValidationPattern),
          ]),
        ],
      },
      {
        validator: this.passwordMatchValidator,
      },
    );
  }

  passwordMatchValidator: ValidatorFn = (
    control: AbstractControl,
  ): { [key: string]: boolean } | null => {
    const newPassword = control.get('newPassword')?.value;
    const confirmPassword = control.get('confirmPassword')?.value;
    return newPassword === confirmPassword ? null : { passwordMismatch: true };
  };

  submitFormChangePassword() {
    this.isSubmitFormChangePassword = true;
    if (this.formChangePassword.valid) {
      this.authService.changePassword(this.formChangePassword.value).subscribe(
        (res: any) => {
          this.isSubmitFormChangePassword = false;
          this.alertService.showMessage(res.message);
          this.formChangePassword.reset();
          this.modalComponent.close();
        },
        (error) => {},
      );
    }
  }

  resetForm() {
    this.isSubmitFormChangePassword = false;
    this.formChangePassword.reset();
  }
}
