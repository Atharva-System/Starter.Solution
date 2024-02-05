import { lazy } from 'react';
const Index = lazy(() => import('../pages/Index'));

const routes = [
    // dashboard
    {
        path: '/',
        element: <Index />,
        layout:''
    }
];

export { routes };
