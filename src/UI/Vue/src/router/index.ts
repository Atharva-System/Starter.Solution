import { createRouter, createWebHistory, type RouteRecordRaw } from 'vue-router';
import { useAppStore } from '@/stores/index';
import appSetting from '@/app-setting';
import { home, signin, forgotPassword, resetPassword, users, projects, tasks, acceptInvitation } from '../common/route-paths';
import UsersView from '../views/apps/user/user-list.vue';
import ProjectsView from '../views/apps/project/project-list.vue';
import TasksView from '../views/apps/task/task-list.vue';
import tokenService from '@/services/token.service';

// Auth Layout Pages
const authLayoutPages: RouteRecordRaw[] = [
    {
        path: signin,
        name: 'sign-in',
        component: () => import(/* webpackChunkName: "auth-boxed-signin" */ '../views/auth/sign-in.vue'),
        meta: { layout: 'auth' },
    },
    {
        path: forgotPassword,
        name: 'forgot-password',
        component: () => import(/* webpackChunkName: "forgot-password" */ '../views/auth/forgot-password.vue'),
        meta: { layout: 'auth' },
    },
    {
        path: resetPassword,
        name: 'reset-password',
        component: () => import('../views/auth/reset-password.vue'),
        meta: { layout: 'auth' },
    },
    {
        path: acceptInvitation,
        name: 'accept-invitation',
        component: () => import('../views/auth/accept-invitation.vue'),
        meta: { layout: 'auth' },
    },
];

// App Layout Pages
const appLayoutPages: RouteRecordRaw[] = [
    { path: home, name: 'home', component: UsersView },
    { path: users, name: 'users', component: UsersView },
    { path: projects, name: 'projects', component: ProjectsView },
    { path: tasks, name: 'tasks', component: TasksView },
];

const routes: RouteRecordRaw[] = [...appLayoutPages, ...authLayoutPages];

const router = createRouter({
    history: createWebHistory(),
    linkExactActiveClass: 'active',
    routes,
    scrollBehavior(to, from, savedPosition) {
        if (savedPosition) {
            return savedPosition;
        } else {
            return { left: 0, top: 0 };
        }
    },
});

router.beforeEach((to, from, next) => {
    const store = useAppStore();

    if (to?.meta?.layout == 'auth') {
        store.setMainLayout('auth');
    } else {
        const token = tokenService.getToken();
        if (token) {
            store.setMainLayout('app');
        } else {
            store.setMainLayout('auth');
            next('/sign-in');
        }
    }
    next(true);
});
router.afterEach((to, from, next) => {
    appSetting.changeAnimation();
});
export default router;
