import { Component, inject } from '@angular/core';
import {
  AbstractControl,
  FormBuilder,
  FormGroup,
  FormsModule,
  ReactiveFormsModule,
  ValidatorFn,
  Validators,
} from '@angular/forms';
import { ActivatedRoute, Router, RouterModule } from '@angular/router';
import { AnchorComponent } from '../../../../shared/ui/anchor/anchor.component';
import { ButtonComponent } from '../../../../shared/ui/button/button.component';
import { InputComponent } from '../../../../shared/ui/input/input.component';
import { authPaths } from '../../../../shared/constants/routes';
import { IResetPasswordRequest } from '../../models/reset-password-request.interface';
import { AuthService } from '../../services/auth.service';
import { AlertService } from '../../../../shared/services/alert.service';
import {
  FieldValidation,
  Regex,
  queryStringParams,
} from '../../../../shared/constants/constants';
import { NgClass } from '@angular/common';

@Component({
  selector: 'app-reset-password',
  standalone: true,
  imports: [
    ReactiveFormsModule,
    InputComponent,
    ButtonComponent,
    AnchorComponent,
    NgClass,
  ],
  templateUrl: './reset-password.component.html',
  styleUrl: './reset-password.component.css',
})
export class ResetPasswordComponent {
  signinRoute = '/' + authPaths.signin;
  formResetPassword!: FormGroup;
  isSubmitFormResetPassword = false;
  forgotPasswordData!: IResetPasswordRequest;

  authService = inject(AuthService);
  alertService = inject(AlertService);
  router = inject(Router);
  fb = inject(FormBuilder);

  constructor(private route: ActivatedRoute) {
    this.initForm();
  }

  initForm() {
    this.formResetPassword = this.fb.group(
      {
        password: [
          '',
          Validators.compose([
            Validators.required,
            Validators.minLength(FieldValidation.passwordMinLength),
            Validators.pattern(Regex.passwordValidationPattern),
          ]),
        ],
        cpassword: [
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
    const password = control.get('password')?.value;
    const cpassword = control.get('cpassword')?.value;
    return password === cpassword ? null : { passwordMismatch: true };
  };

  submitFormResetPassword() {
    this.isSubmitFormResetPassword = true;
    if (this.formResetPassword.valid) {
      var email =
        this.route.snapshot.queryParamMap.get(queryStringParams.Email) ?? '';
      var token =
        this.route.snapshot.queryParamMap.get(queryStringParams.Token) ?? '';
      this.forgotPasswordData = {
        email: email,
        newPassword: this.formResetPassword.value.password,
        token: token,
      };
      this.authService.resetpassword(this.forgotPasswordData).subscribe(
        (res: any) => {
          this.isSubmitFormResetPassword = false;
          this.alertService.showMessage(res.data);
          this.formResetPassword.reset();
          this.router.navigate(['/' + authPaths.signin]);
        },
        (error) => {},
      );
    }
  }
}
