import { Component, inject } from '@angular/core';
import {
  FormGroup,
  FormBuilder,
  Validators,
  FormsModule,
  ReactiveFormsModule,
} from '@angular/forms';
import { Router, RouterModule } from '@angular/router';
import { AuthenticationService } from '../../../../core/services/authentication.service';
import { authPaths } from '../../../../shared/constants/routes';
import { IAuthenticationResponse } from '../../models/authentication-response.interface';
import { AuthService } from '../../services/auth.service';
import { AnchorComponent } from '../../../../shared/ui/anchor/anchor.component';
import { ButtonComponent } from '../../../../shared/ui/button/button.component';
import { InputComponent } from '../../../../shared/ui/input/input.component';

@Component({
  selector: 'app-signin',
  standalone: true,
  imports: [
    RouterModule,
    FormsModule,
    ReactiveFormsModule,
    InputComponent,
    ButtonComponent,
    AnchorComponent,
  ],
  templateUrl: './signin.component.html',
  styleUrl: './signin.component.css',
})
export class SigninComponent {
  forgotPasswordRoute = '/' + authPaths.forgotPassword;
  formSignin!: FormGroup;
  isSubmitFormSignin = false;

  router = inject(Router);
  fb = inject(FormBuilder);
  authService = inject(AuthService);
  authenticationService = inject(AuthenticationService);

  constructor() {
    this.initForm();
  }

  initForm() {
    this.formSignin = this.fb.group({
      email: ['', Validators.compose([Validators.required, Validators.email])],
      password: ['', Validators.required],
    });
  }

  submitFormSignin() {
    this.isSubmitFormSignin = true;
    if (this.formSignin.valid) {
      const formValues = this.formSignin.value;
      this.authService.signin(formValues).subscribe(
        (res: any) => {
          let response = res.data as IAuthenticationResponse;
          this.isSubmitFormSignin = false;
          this.authenticationService.setToken(response.token);
          this.authenticationService.setRefreshToken(response.refreshToken);
          this.router.navigate(['/']);
        },
        (error) => {},
      );
    }
  }
}
