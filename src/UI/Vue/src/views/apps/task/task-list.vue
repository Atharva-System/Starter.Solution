<template>
    <div>
        <div class="panel pb-0">
            <div class="flex md:items-center md:flex-row flex-col mb-5 gap-5">
                <h5 class="font-semibold text-lg dark:text-white-light">Tasks</h5>
                <div class="ltr:ml-auto rtl:mr-auto">
                    <button type="button" class="btn btn-outline-primary btn-sm" @click="openManageTaskModal()">Add</button>
                </div>
            </div>
            <div class="datatable">
                <vue3-datatable :rows="rows" :columns="cols" :loading="loading" :totalRows="total_rows" :isServerMode="true"
                    :pageSize="params.PageSize" :pageSizeOptions="[10, 15, 30, 50]" paginationInfo="{0} to {1} of {2}"
                    :sortable="true" @change="changeServer">
                    <template #statusDisplay="data">
                        <span class="badge" :class="[getBadgeColor(data.value.statusDisplay)]">{{
                            data.value.statusDisplay
                        }}</span>
                    </template>
                    <template #priorityDisplay="data">
                        <span class="badge" :class="[getBadgeColor(data.value.priorityDisplay)]">{{
                            data.value.priorityDisplay
                        }}</span>
                    </template>
                    <template #action="data">
                        <ul class="flex items-center justify-center gap-2">
                            <li>
                                <a href="javascript:;" v-tippy:edit @click="openManageTaskModal(data.value.id)">
                                    <svg width="24" height="24" viewBox="0 0 24 24" fill="none"
                                        xmlns="http://www.w3.org/2000/svg" class="w-4.5 h-4.5 text-success">
                                        <path
                                            d="M15.2869 3.15178L14.3601 4.07866L5.83882 12.5999L5.83881 12.5999C5.26166 13.1771 4.97308 13.4656 4.7249 13.7838C4.43213 14.1592 4.18114 14.5653 3.97634 14.995C3.80273 15.3593 3.67368 15.7465 3.41556 16.5208L2.32181 19.8021L2.05445 20.6042C1.92743 20.9852 2.0266 21.4053 2.31063 21.6894C2.59466 21.9734 3.01478 22.0726 3.39584 21.9456L4.19792 21.6782L7.47918 20.5844L7.47919 20.5844C8.25353 20.3263 8.6407 20.1973 9.00498 20.0237C9.43469 19.8189 9.84082 19.5679 10.2162 19.2751C10.5344 19.0269 10.8229 18.7383 11.4001 18.1612L11.4001 18.1612L19.9213 9.63993L20.8482 8.71306C22.3839 7.17735 22.3839 4.68748 20.8482 3.15178C19.3125 1.61607 16.8226 1.61607 15.2869 3.15178Z"
                                            stroke="currentColor" stroke-width="1.5" />
                                        <path opacity="0.5"
                                            d="M14.36 4.07812C14.36 4.07812 14.4759 6.04774 16.2138 7.78564C17.9517 9.52354 19.9213 9.6394 19.9213 9.6394M4.19789 21.6777L2.32178 19.8015"
                                            stroke="currentColor" stroke-width="1.5" />
                                    </svg>
                                </a>
                            </li>
                            <li>
                                <a href="javascript:;" v-tippy:delete @click="openDeleteTaskModal(data.value.id)">
                                    <svg width="24" height="24" viewBox="0 0 24 24" fill="none"
                                        xmlns="http://www.w3.org/2000/svg" class="w-5 h-5 text-danger">
                                        <path d="M20.5001 6H3.5" stroke="currentColor" stroke-width="1.5"
                                            stroke-linecap="round" />
                                        <path
                                            d="M18.8334 8.5L18.3735 15.3991C18.1965 18.054 18.108 19.3815 17.243 20.1907C16.378 21 15.0476 21 12.3868 21H11.6134C8.9526 21 7.6222 21 6.75719 20.1907C5.89218 19.3815 5.80368 18.054 5.62669 15.3991L5.16675 8.5"
                                            stroke="currentColor" stroke-width="1.5" stroke-linecap="round" />
                                        <path opacity="0.5" d="M9.5 11L10 16" stroke="currentColor" stroke-width="1.5"
                                            stroke-linecap="round" />
                                        <path opacity="0.5" d="M14.5 11L14 16" stroke="currentColor" stroke-width="1.5"
                                            stroke-linecap="round" />
                                        <path opacity="0.5"
                                            d="M6.5 6C6.55588 6 6.58382 6 6.60915 5.99936C7.43259 5.97849 8.15902 5.45491 8.43922 4.68032C8.44784 4.65649 8.45667 4.62999 8.47434 4.57697L8.57143 4.28571C8.65431 4.03708 8.69575 3.91276 8.75071 3.8072C8.97001 3.38607 9.37574 3.09364 9.84461 3.01877C9.96213 3 10.0932 3 10.3553 3H13.6447C13.9068 3 14.0379 3 14.1554 3.01877C14.6243 3.09364 15.03 3.38607 15.2493 3.8072C15.3043 3.91276 15.3457 4.03708 15.4286 4.28571L15.5257 4.57697C15.5433 4.62992 15.5522 4.65651 15.5608 4.68032C15.841 5.45491 16.5674 5.97849 17.3909 5.99936C17.4162 6 17.4441 6 17.5 6"
                                            stroke="currentColor" stroke-width="1.5" />
                                    </svg>
                                </a>
                            </li>
                        </ul>
                    </template>
                </vue3-datatable>
                <TransitionRoot appear :show="isDeleteModal" as="template">
                    <DeleteModal :title="'Delete Task'" :message="'Are you sure you want to delete task?'" @close="onClose"
                        @delete="onDelete" />
                </TransitionRoot>
                <TransitionRoot appear :show="isManageTaskModal" as="template">
                    <ManageskModal :taskId="editTaskId" @close="onClose" @save="onSave" />
                </TransitionRoot>
            </div>
        </div>
    </div>
</template>
<script lang="ts">
import { TransitionRoot } from '@headlessui/vue';
import Vue3Datatable from '@bhplugin/vue3-datatable';
import { deleteTaskApi, searchTasksApi } from '@/common/api-paths';
import { useMeta } from '@/composables/use-meta';
import api from '@/services/api';
import messageService from '@/services/message.service';
import { PaginationFilter } from '@/types/pagination-filter.interface';
import DeleteModal from '../../components/delete-modal.vue';
import ManageskModal from './manage-task-modal.vue';
import filterService from '@/services/filter.service';

useMeta({ title: 'Tasks' });

export default {
    components: {
        Vue3Datatable,
        TransitionRoot,
        ManageskModal,
        DeleteModal
    },
    setup() {
        return {
            cols: [
                { field: 'taskName', title: 'Task' },
                { field: 'projectName', title: 'Project' },
                { field: 'startDateDisplay', title: 'Start Date' },
                { field: 'endDateDisplay', title: 'End Date' },
                { field: 'statusDisplay', title: 'Status' },
                { field: 'priorityDisplay', title: 'Priority' },
                {
                    field: 'action',
                    title: 'Action',
                    sort: false,
                    filter: false,
                    headerClass: 'justify-center',
                },
            ] || []
        }
    },
    data() {
        return {
            isDeleteModal: false,
            isManageTaskModal: false,
            loading: true,
            params: {} as PaginationFilter,
            rows: [],
            total_rows: 0,
            timer: null as any,
            deleteTaskId: '',
            editTaskId: '',
        }
    },
    created() {
        this.params = filterService.defaultFilter();
        this.getTasks();
    }, methods: {
        async getTasks() {
            this.loading = true
            const response = await api
                .post(searchTasksApi, this.params);
            if (response.data) {
                this.rows = response.data.data;
                this.total_rows = response.data.totalCount
                this.loading = false
            }
        },
        changeServer(data: any) {
            this.params.PageNumber = data.current_page;
            this.params.PageSize = data.pagesize;
            this.params.OrderBy = [
                this.getSortColumnName(data.sort_column) + ' ' + data.sort_direction,
            ];
            this.getTasks();
        },
        getBadgeColor(status: string): string {
            switch (status) {
                case 'High':
                    return 'badge-outline-success';
                case 'Low':
                    return 'badge-outline-danger';
                case 'To Do':
                    return 'bg-secondary';
                case 'Completed':
                    return 'bg-success';
                case 'In Progress':
                    return 'bg-primary';
                default:
                    return 'badge-outline-info';
            }
        },

        getSortColumnName(column: string): string {
            switch (column) {
                case 'statusDisplay':
                    return 'status';
                case 'priorityDisplay':
                    return 'priority';
                case 'startDateDisplay':
                    return 'startDate';
                case 'endDateDisplay':
                    return 'endDate';
                default:
                    return column;
            }
        },
        openManageTaskModal(id: string = '') {
            this.editTaskId = id
            this.isManageTaskModal = true
        },
        openDeleteTaskModal(id: string) {
            this.deleteTaskId = id;
            this.isDeleteModal = true
        },
        async onSave() {
            this.isManageTaskModal = false
            this.getTasks();
        },
        async onDelete() {
            const response = await api
                .delete(deleteTaskApi + this.deleteTaskId);
            if (response.data) {
                messageService.showMessage(response.data.message);
                this.isDeleteModal = false
                this.getTasks();
            }
        },
        onClose() {
            this.isDeleteModal = this.isManageTaskModal = false
            setTimeout(() => {
                this.deleteTaskId = this.editTaskId = '';
            }, 200);
        }
    }
}
</script>
