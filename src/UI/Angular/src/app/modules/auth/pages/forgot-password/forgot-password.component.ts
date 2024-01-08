import { Component, inject } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from '../../services/auth.service';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ButtonComponent } from '../../../../shared/ui/button/button.component';
import { InputComponent } from '../../../../shared/ui/input/input.component';
import { AlertService } from '../../../../shared/services/alert.service';
import { AnchorComponent } from '../../../../shared/ui/anchor/anchor.component';
import { authPaths } from '../../../../shared/constants/routes';

@Component({
  selector: 'app-forgot-password',
  standalone: true,
  imports: [InputComponent, ButtonComponent, AnchorComponent],
  templateUrl: './forgot-password.component.html',
  styleUrl: './forgot-password.component.css',
})
export class ForgotPasswordComponent {
  signinRoute = '/' + authPaths.signin;
  formForgotPassword!: FormGroup;
  isSubmitFormForgotPassword = false;

  router = inject(Router);
  authService = inject(AuthService);
  fb = inject(FormBuilder);
  alertService = inject(AlertService);

  constructor() {
    this.initForm();
  }

  initForm() {
    this.formForgotPassword = this.fb.group({
      email: ['', Validators.compose([Validators.required, Validators.email])],
    });
  }

  recover() {
    this.isSubmitFormForgotPassword = true;
    if (this.formForgotPassword.valid) {
      const formValues = this.formForgotPassword.value;
      this.authService.forgotpassword(formValues.email).subscribe((res) => {
        if (res.success) {
          this.isSubmitFormForgotPassword = false;
          this.alertService.showMessage(res.data);
          this.formForgotPassword.reset();
        }
      });
    }
  }
}
