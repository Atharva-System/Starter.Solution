import { Routes } from '@angular/router';
import { AuthGuard } from './core/guard/auth.guard';
import { AppLayout } from './core/layouts/app-layout';
import { AuthLayout } from './core/layouts/auth-layout';
import { SignupComponent } from './module/auth/pages/signup/signup.component';
import { appPaths, authPaths, pageTitle } from './shared/constants/routes';
import { ForgotPasswordComponent } from './module/auth/pages/forgot-password/forgot-password.component';
import { ListUsersComponent } from './module/user/pages/list-users/list-users.component';
import { SigninComponent } from './module/auth/pages/signin/signin.component';
import { AcceptInvitationComponent } from './module/auth/pages/accept-invitation/accept-invitation.component';
import { ResetPasswordComponent } from './module/auth/pages/reset-password/reset-password.component';
import { ProfileComponent } from './module/user/pages/profile/profile.component';

export const routes: Routes = [
  {
    path: '',
    component: AppLayout,
    children: [
      {
        path: '',
        component: ListUsersComponent,
        title: pageTitle.users,
      },
      {
        path: appPaths.users,
        component: ListUsersComponent,
        title: pageTitle.users,
      },
      {
        path: appPaths.profile,
        component: ProfileComponent,
        title: pageTitle.profile,
      },
    ],
    canActivate: [AuthGuard],
  },
  {
    path: '',
    component: AuthLayout,
    children: [
      {
        path: authPaths.signin,
        component: SigninComponent,
        title: pageTitle.signin,
      },
      {
        path: authPaths.signup,
        component: SignupComponent,
        title: pageTitle.signup,
      },
      {
        path: authPaths.forgotPassword,
        component: ForgotPasswordComponent,
        title: pageTitle.forgotPassword,
      },
      {
        path: authPaths.acceptInvitation,
        component: AcceptInvitationComponent,
        title: pageTitle.acceptInvitation,
      },
      {
        path: authPaths.resetPassword,
        component: ResetPasswordComponent,
        title: pageTitle.resetPassword,
      },
    ],
  },
];
