import { projects, tasks, users } from '@/common/route-paths';

class MenuService {
    getMenus(): Array<{ label: string; link: string }> {
        const menuItems: Array<{ label: string; link: string }> = [];
        menuItems.push({
            label: 'Users',
            link: users,
        });

        menuItems.push({
            label: 'Projects',
            link: projects,
        });

        menuItems.push({
            label: 'Tasks',
            link: tasks,
        });
        return menuItems;
    }
}
export default new MenuService();
