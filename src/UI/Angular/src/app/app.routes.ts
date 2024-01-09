import { Routes } from '@angular/router';
import { AuthGuard } from './core/guard/auth.guard';
import { AppLayout } from './core/layouts/app-layout';
import { AuthLayout } from './core/layouts/auth-layout';
import { SignupComponent } from './modules/auth/pages/signup/signup.component';
import { appPaths, authPaths, pageTitle } from './shared/constants/routes';
import { ForgotPasswordComponent } from './modules/auth/pages/forgot-password/forgot-password.component';
import { ListUsersComponent } from './modules/user/pages/list-users/list-users.component';
import { SigninComponent } from './modules/auth/pages/signin/signin.component';
import { AcceptInvitationComponent } from './modules/auth/pages/accept-invitation/accept-invitation.component';
import { ResetPasswordComponent } from './modules/auth/pages/reset-password/reset-password.component';
import { ProfileComponent } from './modules/user/pages/profile/profile.component';
import { ListProjectsComponent } from './modules/project/pages/list-projects/list-projects.component';
import { ListTasksComponent } from './modules/task/pages/list-tasks/list-tasks.component';

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
      {
        path: appPaths.projects,
        component: ListProjectsComponent,
        title: pageTitle.projects,
      },
      {
        path: appPaths.tasks,
        component: ListTasksComponent,
        title: pageTitle.tasks,
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
