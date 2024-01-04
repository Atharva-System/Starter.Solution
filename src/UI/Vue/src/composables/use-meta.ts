import { useHead } from '@vueuse/head';
import { unref, computed } from 'vue';

const siteTitle = '';
const separator = '|';

interface PageTitleOptions {
    title: string;
}

export const usePageTitle = (pageTitle: string): void => {
    useHead(
        computed<PageTitleOptions>(() => ({
            title: `${unref(pageTitle)} ${separator} ${siteTitle}`,
        })),
    );
};

// export const usePageTitle = (pageTitle: any) => {
//     useHead(
//         computed(() => ({
//             title: `${unref(pageTitle)} ${separator} ${siteTitle}`,
//         })),
//     );
// };


export const useMeta = (data: any) => {
    return useHead(
        { ...data, title: `${data.title} | Starter - To-Do` }
    );
};