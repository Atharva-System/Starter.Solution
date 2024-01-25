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
                        {{ projectId ? 'Edit Project' : 'Add Project' }}
                    </div>
                    <div class="p-5">
                        <form @submit.prevent="submitForm()">
                            <div class="mb-5" :class="{ 'has-error': v$.params.projectName.$error }">
                                <label for="projectName">Project Name</label>
                                <input id="projectName" type="text" placeholder="Enter Project Name" class="form-input"
                                    v-model="params.projectName" />
                                <template v-if="isSubmitForm && v$.params.projectName.$error">
                                    <p class="text-danger mt-1">This can not be empty</p>
                                </template>
                            </div>
                            <div class="mb-5">
                                <label for="description">Description</label>
                                <quillEditor ref="editor1" v-model:value="params.description" :options="options1"
                                    style="min-height: 150px"></quillEditor>
                            </div>
                            <div class="mb-5" :class="{ 'has-error': v$.params.deadline.$error }">
                                <label for="deadline">Deadline</label>
                                <flat-pickr v-model="params.deadline" class="form-input" placeholder="Enter Deadline"
                                    :config="rangeCalendar"></flat-pickr>
                                <template v-if="isSubmitForm && v$.params.deadline.$error">
                                    <p class="text-danger mt-1">This can not be empty</p>
                                </template>
                            </div>
                            <div class="mb-5" :class="{ 'has-error': v$.params.estimatedHours.$error }">
                                <label for="estimatedHours">Estimated Hours</label>
                                <input id="estimatedHours" type="number" placeholder="Enter Estimated Hours"
                                    class="form-input" v-model="params.estimatedHours" />
                                <template v-if="isSubmitForm && v$.params.estimatedHours.$error">
                                    <p class="text-danger mt-1">This can not be empty</p>
                                </template>
                            </div>
                            <div class="flex justify-end items-center mt-8">
                                <button type="button" @click="onClose" class="btn btn-outline-danger">Discard</button>
                                <button type="submit" class="btn btn-primary ltr:ml-4 rtl:mr-4" :disabled="isSaveDisabled">
                                    Save
                                </button>
                            </div>
                        </form>
                    </div>
                </DialogPanel>
            </div>
        </div>
    </Dialog>
</template>
<script lang="ts">
import { createProjectApi, getProjectApi, updateProjectApi } from '@/common/api-paths';
import api from '@/services/api';
import messageService from '@/services/message.service';
import { useAppStore } from '@/stores';
import { TransitionChild, Dialog, DialogPanel, DialogOverlay } from '@headlessui/vue';
import useVuelidate from '@vuelidate/core';
import { required } from '@vuelidate/validators';
import { PropType, ref } from 'vue';
import { quillEditor } from 'vue3-quill';
import 'vue3-quill/lib/vue3-quill.css';
import flatPickr from 'vue-flatpickr-component';
import 'flatpickr/dist/flatpickr.css';
import commonService from '@/services/common.service';

export default {
    components: {
        quillEditor, flatPickr, TransitionChild, Dialog, DialogPanel, DialogOverlay
    },
    props: {
        projectId: {
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
            isSaveDisabled: false,
            isSubmitForm: false,
            params: {
                projectName: '',
                description: '',
                deadline: '',
                estimatedHours: ''
            },
            originalParams: {
                projectName: '',
                description: '',
                deadline: '',
                estimatedHours: ''
            },
            options1: ref({
                modules: {
                    toolbar: [
                        [{ header: [1, 2, false] }],
                        ['bold', 'italic', 'underline', 'link'],
                        [{ list: 'ordered' }, { list: 'bullet' }],
                        ['clean'],
                    ],
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
            params: { projectName: { required }, deadline: { required }, estimatedHours: { required }, },
        }
    },
    created() {
        if (this.projectId) {
            this.setProjectDetails();
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
            const response = this.projectId
                ? await api
                    .put(updateProjectApi + this.projectId, {
                        id: this.projectId,
                        projectName: this.params.projectName,
                        description: this.params.description,
                        startDate: dates.startDate,
                        endDate: dates.endDate,
                        estimatedHours: this.params.estimatedHours
                    })
                : await api
                    .post(createProjectApi, {
                        projectName: this.params.projectName,
                        description: this.params.description,
                        startDate: dates.startDate,
                        endDate: dates.endDate,
                        estimatedHours: this.params.estimatedHours
                    });

            if (response.data) {
                messageService.showMessage(response.data.data);
                this.onSave();
            }
        },
        async setProjectDetails() {
            const response = await api
                .get(getProjectApi + this.projectId);
            if (response.data) {
                this.originalParams = {
                    projectName: response.data.data.projectName,
                    description: response.data.data.description,
                    deadline: response.data.data.startDate + ' to ' + response.data.data.endDate,
                    estimatedHours: response.data.data.estimatedHours
                }
                this.params = {
                    projectName: response.data.data.projectName,
                    description: response.data.data.description,
                    deadline: response.data.data.startDate + ' to ' + response.data.data.endDate,
                    estimatedHours: response.data.data.estimatedHours
                }
            }
        },
        isParamsUnchanged(newParams) {
            return JSON.stringify(newParams) == JSON.stringify(this.originalParams);
        },
    }
};
</script>