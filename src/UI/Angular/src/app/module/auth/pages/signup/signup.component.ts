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
      name: ['', Validators.required],
      email: ['', Validators.compose([Validators.required, Validators.email])],
      password: ['', Validators.required],
    });
  }

  submitFormSignup() {
    this.isSubmitFormSignup = true;
    if (this.formSignup.valid) {
      const formValues = this.formSignup.value;
      console.log('Form Values:', formValues);
      this.alertService.showMessage('Form submitted successfully.');
    }
  }
}
