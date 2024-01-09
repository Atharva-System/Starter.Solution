import { Component, inject } from '@angular/core';
import { Router, RouterModule } from '@angular/router';
import { authPaths } from '../../../../shared/constants/routes';
import {
  FormGroup,
  FormBuilder,
  Validators,
  ReactiveFormsModule,
  FormsModule,
} from '@angular/forms';
import { AlertService } from '../../../../shared/services/alert.service';
import { InputComponent } from '../../../../shared/ui/input/input.component';
import { ButtonComponent } from '../../../../shared/ui/button/button.component';
import { AnchorComponent } from '../../../../shared/ui/anchor/anchor.component';
import { AuthService } from '../../services/auth.service';
import { FieldValidation } from '../../../../shared/constants/constants';
import { TranslateModule } from '@ngx-translate/core';

@Component({
  selector: 'app-signup',
  templateUrl: './signup.component.html',
  styleUrl: './signup.component.css',
  standalone: true,
  imports: [
    RouterModule,
    FormsModule,
    ReactiveFormsModule,
    InputComponent,
    ButtonComponent,
    AnchorComponent,
  ],
})
export class SignupComponent {
  signinRoute = '/' + authPaths.signin;
  formSignup!: FormGroup;
  isSubmitFormSignup = false;

  router = inject(Router);
  fb = inject(FormBuilder);
  alertService = inject(AlertService);
  authService = inject(AuthService);

  constructor() {
    this.initForm();
  }

  initForm() {
    this.formSignup = this.fb.group({
      firstName: [
        '',
        Validators.compose([
          Validators.required,
          Validators.maxLength(FieldValidation.firstNameMaxLength),
        ]),
      ],
      lastName: [
        '',
        Validators.compose([
          Validators.required,
          Validators.maxLength(FieldValidation.lastNameMaxLength),
        ]),
      ],
      email: [
        '',
        Validators.compose([
          Validators.required,
          Validators.email,
          Validators.maxLength(FieldValidation.emailMaxLength),
        ]),
      ],
      password: [
        '',
        Validators.compose([
          Validators.required,
          Validators.minLength(FieldValidation.passwordMinLength),
          // Validators.pattern(Regex.passwordValidationPattern),
        ]),
      ],
    });
  }

  submitFormSignup() {
    this.isSubmitFormSignup = true;
    if (this.formSignup.valid) {
      const formValues = this.formSignup.value;
      this.authService.signup(formValues).subscribe(
        (res: any) => {
          this.isSubmitFormSignup = false;
          this.alertService.showMessage('User registered successfully.');
          this.formSignup.reset();
        },
        (error) => {},
      );
    }
  }
}
