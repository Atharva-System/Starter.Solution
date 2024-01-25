<template>
    <Dialog as="div" class="relative z-50">
        <TransitionChild as="template" enter="duration-300 ease-out" enter-from="opacity-0" enter-to="opacity-100"
            leave="duration-200 ease-in" leave-from="opacity-100" leave-to="opacity-0">
            <DialogOverlay class="fixed inset-0 bg-[black]/60" />
        </TransitionChild>

        <div class="fixed inset-0 overflow-y-auto">
            <div class="flex min-h-full items-center justify-center px-4 py-8">
                <DialogPanel
                    class="panel border-0 p-0 rounded-lg overflow-hidden w-full max-w-lg text-black dark:text-white-dark animate__animated animate__fadeIn">
                    <button type="button"
                        class="absolute top-4 ltr:right-4 rtl:left-4 text-gray-400 hover:text-gray-800 dark:hover:text-gray-600 outline-none"
                        @click="onClose">
                        <svg xmlns="http://www.w3.org/2000/svg" width="24px" height="24px" viewBox="0 0 24 24" fill="none"
                            stroke="currentColor" stroke-width="1.5" stroke-linecap="round" stroke-linejoin="round"
                            class="w-6 h-6">
                            <line x1="18" y1="6" x2="6" y2="18"></line>
                            <line x1="6" y1="6" x2="18" y2="18"></line>
                        </svg>
                    </button>
                    <div
                        class="text-lg font-bold bg-[#fbfbfb] dark:bg-[#121c2c] ltr:pl-5 rtl:pr-5 py-3 ltr:pr-[50px] rtl:pl-[50px]">
                        {{ taskId ? 'Edit Task' : 'Add Task' }}
                    </div>
                    <div class="p-5">
                        <form @submit.prevent="submitForm">
                            <div class="mb-5" :class="{ 'has-error': v$.params.taskName.$error }">
                                <label for="taskName">Title</label>
                                <input id="taskName" type="text" placeholder="Enter Task Title" class="form-input"
                                    v-model="params.taskName" />
                                <template v-if="isSubmitForm && v$.params.taskName.$error">
                                    <p class="text-danger mt-1">This can not be empty</p>
                                </template>
                            </div>
                            <div class="mb-5 flex justify-between gap-4">
                                <div class="flex-1" :class="{ 'has-error': v$.params.projectId.$error }">
                                    <label for="projectId">Project</label>
                                    <select id="projectId" class="form-select" v-model="params.projectId">
                                        <option value="">Select Project</option>
                                        <template v-for="item in projectsOptions" :key="item.value">
                                            <option :value="item.value">{{ item.label }}</option>
                                        </template>
                                    </select>
                                    <template v-if="isSubmitForm && v$.params.projectId.$error">
                                        <p class="text-danger mt-1">This can not be empty</p>
                                    </template>
                                </div>
                                <div class="flex-1" :class="{ 'has-error': v$.params.assignedTo.$error }">
                                    <label for="assignedTo">Assignee</label>
                                    <select id="assignedTo" class="form-select" v-model="params.assignedTo">
                                        <option value="">Select Assignee</option>
                                        <template v-for="item in assignToUsersOptions" :key="item.value">
                                            <option :value="item.value">{{ item.label }}</option>
                                        </template>
                                    </select>
                                    <template v-if="isSubmitForm && v$.params.assignedTo.$error">
                                        <p class="text-danger mt-1">This can not be empty</p>
                                    </template>
                                </div>
                            </div>
                            <div class="mb-5 flex justify-between gap-4">
                                <div class="flex-1" :class="{ 'has-error': v$.params.status.$error }">
                                    <label for="status">Status</label>
                                    <select id="status" class="form-select" v-model="params.status">
                                        <option value="">Select Status</option>
                                        <template v-for="item in statusOptions" :key="item.value">
                                            <option :value="item.value">{{ item.label }}</option>
                                        </template>
                                    </select>
                                    <template v-if="isSubmitForm && v$.params.status.$error">
                                        <p class="text-danger mt-1">This can not be empty</p>
                                    </template>
                                </div>
                                <div class="flex-1" :class="{ 'has-error': v$.params.priority.$error }">
                                    <label for="priority">Priority</label>
                                    <select id="priority" class="form-select" v-model="params.priority">
                                        <option value="">Select Priority</option>
                                        <template v-for="item in priorityOptions" :key="item.value">
                                            <option :value="item.value">{{ item.label }}</option>
                                        </template>
                                    </select>
                                    <template v-if="isSubmitForm && v$.params.priority.$error">
                                        <p class="text-danger mt-1">This can not be empty</p>
                                    </template>
                                </div>
                            </div>
                            <div class="mb-5" :class="{ 'has-error': v$.params.deadline.$error }">
                                <label for="deadline">Deadline</label>
                                <flat-pickr v-model="params.deadline" class="form-input" placeholder="Enter Deadline"
                                    :config="rangeCalendar"></flat-pickr>
                                <template v-if="isSubmitForm && v$.params.deadline.$error">
                                    <p class="text-danger mt-1">This can not be empty</p>
                                </template>
                            </div>
                            <div class="mb-5">
                                <label>Description</label>
                                <quillEditor ref="editor" v-model:value="params.description" :options="editorOptions"
                                    style="min-height: 200px"></quillEditor>
                            </div>
                            <div class="ltr:text-right rtl:text-left flex justify-end items-center mt-8">
                                <button type="button" class="btn btn-outline-danger" @click="onClose">Cancel</button>
                                <button type="submit" class="btn btn-primary ltr:ml-4 rtl:mr-4"
                                    :disabled="isSaveDisabled">{{ taskId ? 'Update' : 'Add'
                                    }}</button>
                            </div>
                        </form>
                    </div>
                </DialogPanel>
            </div>
        </div>
    </Dialog>
</template>
<script lang="ts">
import { createTaskApi, getAssigneeListApi, getProjectListApi, getTaskApi, getTaskPriorityListApi, getTaskStatusListApi, updateTaskApi } from '@/common/api-paths';
import api from '@/services/api';
import messageService from '@/services/message.service';
import { TransitionChild, Dialog, DialogPanel, DialogOverlay } from '@headlessui/vue';
import useVuelidate from '@vuelidate/core';
import { required } from '@vuelidate/validators';
import { PropType, ref } from 'vue';
import { quillEditor } from 'vue3-quill';
import 'vue3-quill/lib/vue3-quill.css';
import flatPickr from 'vue-flatpickr-component';
import 'flatpickr/dist/flatpickr.css';
import commonService from '@/services/common.service';
import { ISelectItems } from '@/types';
import { useAppStore } from '@/stores';

export default {
    components: {
        flatPickr, quillEditor, TransitionChild, Dialog, DialogPanel, DialogOverlay
    },
    props: {
        taskId: {
            type: String,
            default: '',
        },
        onSave: {
            type: Function as PropType<() => void>,
            required: true,
        },
        onClose: {
            type: Function as PropType<() => void>,
            required: true,
        },
    },
    setup() {
        return {
            v$: useVuelidate()
        }
    },
    data() {
        return {
            projectsOptions: [] as ISelectItems[],
            assignToUsersOptions: [] as ISelectItems[],
            statusOptions: [] as ISelectItems[],
            priorityOptions: [] as ISelectItems[],
            isSaveDisabled: false,
            isSubmitForm: false,
            params: { taskName: '', projectId: '', assignedTo: '', status: '', priority: '', deadline: '', description: '' },
            originalParams: { taskName: '', projectId: '', assignedTo: '', status: '', priority: '', deadline: '', description: '' },
            editorOptions: ref({
                modules: {
                    toolbar: [[{ header: [1, 2, false] }], ['bold', 'italic', 'underline', 'link'], [{ list: 'ordered' }, { list: 'bullet' }], ['clean']],
                },
                placeholder: '',
            }),
            rangeCalendar: ref({
                dateFormat: 'Y-m-d',
                mode: 'range',
                position: useAppStore().rtlClass === 'rtl' ? 'auto right' : 'auto left',
            }) as any
        }
    },
    validations() {
        return {
            params: {
                taskName: { required },
                projectId: { required },
                assignedTo: { required },
                status: { required },
                priority: { required },
                deadline: { required },
            },
        }
    },
    created() {
        this.bindProjects()
        this.bindAssignee()
        this.bindStatus()
        this.bindPriority()
        if (this.taskId) {
            this.setTaskDetails();
        }
    },
    watch: {
        params: {
            deep: true,
            handler(newParams) {
                this.isSaveDisabled = this.isParamsUnchanged(newParams);
            },
        },
    },
    methods: {
        async submitForm() {
            this.isSubmitForm = true
            const isFormCorrect = await this.v$.$validate()
            if (!isFormCorrect) return

            const dates = commonService.parseDateRange(this.params.deadline);

            const response = this.taskId
                ? await api
                    .put(updateTaskApi + this.taskId, {
                        id: this.taskId,
                        taskName: this.params.taskName,
                        projectId: this.params.projectId,
                        assignedTo: this.params.assignedTo,
                        status: this.params.status,
                        priority: this.params.priority,
                        startDate: dates.startDate,
                        endDate: dates.endDate,
                        description: this.params.description
                    })
                : await api
                    .post(createTaskApi, {
                        taskName: this.params.taskName,
                        projectId: this.params.projectId,
                        assignedTo: this.params.assignedTo,
                        status: this.params.status,
                        priority: this.params.priority,
                        startDate: dates.startDate,
                        endDate: dates.endDate,
                        description: this.params.description
                    });

            if (response.data) {
                messageService.showMessage(response.data.message);
                this.onSave();
            }
        },
        async setTaskDetails() {
            const response = await api
                .get(getTaskApi + this.taskId);
            if (response.data) {
                this.originalParams = {
                    taskName: response.data.data.taskName,
                    projectId: response.data.data.projectId,
                    assignedTo: response.data.data.assignedTo,
                    status: response.data.data.status,
                    priority: response.data.data.priority,
                    deadline: response.data.data.startDate + ' to ' + response.data.data.endDate,
                    description: response.data.data.description
                }
                this.params = {
                    taskName: response.data.data.taskName,
                    projectId: response.data.data.projectId,
                    assignedTo: response.data.data.assignedTo,
                    status: response.data.data.status,
                    priority: response.data.data.priority,
                    deadline: response.data.data.startDate + ' to ' + response.data.data.endDate,
                    description: response.data.data.description
                }
            }
        },
        isParamsUnchanged(newParams) {
            return JSON.stringify(newParams) == JSON.stringify(this.originalParams);
        },
        async bindProjects() {
            var response = await api
                .get(getProjectListApi);

            if (response.data) {
                this.projectsOptions = response.data.data.map(
                    ({ id, projectName }: { id: any; projectName: string }) => ({
                        value: `${id}`,
                        label: projectName,
                    }),
                );
            }
        },
        async bindAssignee() {
            var response = await api
                .get(getAssigneeListApi);

            if (response.data) {
                this.assignToUsersOptions = response.data.data.map(
                    ({ id, name }: { id: any; name: string }) => ({
                        value: `${id}`,
                        label: name,
                    }),
                );
            }
        },
        async bindStatus() {
            var response = await api
                .get(getTaskStatusListApi);

            if (response.data) {
                this.statusOptions = response.data.data.map(
                    ({ id, name }: { id: any; name: string }) => ({
                        value: `${id}`,
                        label: name,
                    }),
                );
            }
        },
        async bindPriority() {
            var response = await api
                .get(getTaskPriorityListApi);

            if (response.data) {
                this.priorityOptions = response.data.data.map(
                    ({ id, name }: { id: any; name: string }) => ({
                        value: `${id}`,
                        label: name,
                    }),
                );
            }
        }
    }
};
</script>