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
                        {{ userId ? 'Edit User' : 'Invite User' }}
                    </div>
                    <div class="p-5">
                        <form @submit.prevent="submitForm()">
                            <div class="mb-5 flex justify-between gap-4">
                                <div class="flex-1" :class="{ 'has-error': v$.params.firstName.$error }">
                                    <label for="firstName">First Name</label>
                                    <input id="firstName" type="text" placeholder="Enter First Name" class="form-input"
                                        v-model="params.firstName" />
                                    <template v-if="isSubmitForm && v$.params.firstName.$error">
                                        <p class="text-danger mt-1" v-for="error of v$.params.firstName.$errors"
                                            :key="error.$uid">
                                            <span v-if="error.$validator == 'required'">This can not be empty</span>
                                            <span v-if="error.$validator == 'noSpace'">Invalid First Name</span>
                                        </p>
                                    </template>
                                </div>
                                <div class="flex-1" :class="{ 'has-error': v$.params.lastName.$error }">
                                    <label for="lastName">Last Name</label>
                                    <input id="lastName" type="text" placeholder="Enter Last Name" class="form-input"
                                        v-model="params.lastName" />
                                    <template v-if="isSubmitForm && v$.params.lastName.$error">
                                        <p class="text-danger mt-1" v-for="error of v$.params.lastName.$errors"
                                            :key="error.$uid">
                                            <span v-if="error.$validator == 'required'">This can not be empty</span>
                                            <span v-if="error.$validator == 'noSpace'">Invalid Last Name</span>
                                        </p>
                                    </template>
                                </div>
                            </div>
                            <div class="mb-5" :class="{ 'has-error': v$.params.email.$error }">
                                <label for="email">Email</label>
                                <input id="email" type="text" placeholder="Enter Email" class="form-input"
                                    v-model="params.email" />
                                <template v-if="isSubmitForm && v$.params.email.$error">
                                    <p class="text-danger mt-1" v-for="error of v$.params.email.$errors" :key="error.$uid">
                                        <span v-if="error.$validator == 'required'">This can not be empty</span>
                                        <span v-if="error.$validator == 'email'">Invalid Email</span>
                                    </p>
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
import { getUserApi, inviteUserApi, updateUserApi } from '@/common/api-paths';
import { noSpaceValidationPattern } from '@/common/constants';
import api from '@/services/api';
import messageService from '@/services/message.service';
import { TransitionChild, Dialog, DialogPanel, DialogOverlay } from '@headlessui/vue';
import useVuelidate from '@vuelidate/core';
import { email, required } from '@vuelidate/validators';
import { PropType } from 'vue';

export default {
    components: {
        TransitionChild, Dialog, DialogPanel, DialogOverlay
    },
    props: {
        userId: {
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
            params: { firstName: '', lastName: '', email: '', },
            originalParams: { firstName: '', lastName: '', email: '', },
        }
    },
    validations() {
        return {
            params: {
                firstName: { required, noSpace: value => noSpaceValidationPattern.test(value), },
                lastName: { required, noSpace: value => noSpaceValidationPattern.test(value), },
                email: { required, email },
            },
        }
    },
    created() {
        if (this.userId) {
            this.setUserDetails();
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

            const response = this.userId
                ? await api
                    .put(updateUserApi + this.userId, { id: this.userId, firstName: this.params.firstName, lastName: this.params.lastName, email: this.params.email })
                : await api
                    .post(inviteUserApi, this.params);

            if (response.data) {
                messageService.showMessage(response.data.data);
                this.onSave();
            }
        },
        async setUserDetails() {
            const response = await api
                .get(getUserApi + this.userId);
            if (response.data) {
                this.originalParams = { firstName: response.data.data.firstName, lastName: response.data.data.lastName, email: response.data.data.email }
                this.params = { firstName: response.data.data.firstName, lastName: response.data.data.lastName, email: response.data.data.email }
            }
        },
        isParamsUnchanged(newParams) {
            return JSON.stringify(newParams) == JSON.stringify(this.originalParams);
        },
    }
};
</script>