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
                        Change Pasword
                    </div>
                    <div class="p-5">
                        <form @submit.prevent="submitForm()">
                            <div class="mb-5" :class="{ 'has-error': v$.params.currentPassword.$error }">
                                <label for="currentPassword">Current Password</label>
                                <input id="currentPassword" type="password" placeholder="Enter Current Password"
                                    class="form-input" v-model="params.currentPassword" />
                                <template v-if="isSubmitForm && v$.params.currentPassword.$error">
                                    <p class="text-danger mt-1">This can not be empty</p>
                                </template>
                            </div>
                            <div class="mb-5" :class="{ 'has-error': v$.params.newPassword.$error }">
                                <label for="newPassword">New Password</label>
                                <input id="newPassword" type="password" placeholder="Enter New Password" class="form-input"
                                    v-model="params.newPassword" />
                                <template v-if="isSubmitForm && v$.params.newPassword.$error">
                                    <p class="text-danger mt-1" v-for="error of v$.params.newPassword.$errors"
                                        :key="error.$uid">
                                        <span v-if="error.$validator == 'required'">This can not be empty</span>
                                        <span v-if="error.$validator == 'minLength'">Minimum length should be {{
                                            v$.params.newPassword.minLength.$params.min }} characters</span>
                                        <span v-if="error.$validator == 'passPatern'">Password requirements: At least one
                                            uppercase
                                            letter (A-Z), one lowercase
                                            letter (a-z), and one non-alphanumeric character.</span>
                                    </p>
                                </template>
                            </div>
                            <div class="mb-5" :class="{ 'has-error': v$.params.confirmPassword.$error }">
                                <label for="confirmPassword">Confirm Password</label>
                                <input id="confirmPassword" type="password" placeholder="Confirm New Password"
                                    class="form-input" v-model="params.confirmPassword" />
                                <template v-if="isSubmitForm && v$.params.confirmPassword.required.$invalid">
                                    <p class="text-danger mt-1">This can not be empty</p>
                                </template>
                                <template v-if="isSubmitForm && v$.params.confirmPassword.minLength.$invalid">
                                    <p class="text-danger mt-1">Minimum length should be {{
                                        v$.params.confirmPassword.minLength.$params.min }} characters</p>
                                </template>
                                <template v-if="isSubmitForm && v$.params.confirmPassword.passPatern.$invalid">
                                    <p class="text-danger mt-1">Password requirements: At least one uppercase
                                        letter (A-Z), one lowercase
                                        letter (a-z), and one non-alphanumeric character.</p>
                                </template>
                                <template v-if="isSubmitForm && v$.params.confirmPassword.sameAsPassword.$invalid">
                                    <p class="text-danger mt-1">Password do not match</p>
                                </template>
                            </div>
                            <div class="flex justify-end items-center mt-8">
                                <button type="button" @click="onClose" class="btn btn-outline-danger">Discard</button>
                                <button type="submit" class="btn btn-primary ltr:ml-4 rtl:mr-4">
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
import { changePasswordApi } from '@/common/api-paths';
import { passwordValidationPattern } from '@/common/constants';
import api from '@/services/api';
import messageService from '@/services/message.service';
import { TransitionChild, Dialog, DialogPanel, DialogOverlay } from '@headlessui/vue';
import useVuelidate from '@vuelidate/core';
import { minLength, required } from '@vuelidate/validators';
import { PropType } from 'vue';

export default {
    components: {
        TransitionChild, Dialog, DialogPanel, DialogOverlay
    }, props: {
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
            isSubmitForm: false,
            params: { currentPassword: '', newPassword: '', confirmPassword: '', },
        }
    },
    validations() {
        return {
            params: {
                currentPassword: { required },
                newPassword: { required, minLength: minLength(6), passPatern: value => passwordValidationPattern.test(value) },
                confirmPassword: {
                    required: required,
                    minLength: minLength(6),
                    passPatern: value => passwordValidationPattern.test(value),
                    sameAsPassword: (value: any, vm: any) => value === vm.newPassword,
                },
            },
        }
    },
    methods: {
        async submitForm() {
            this.isSubmitForm = true
            const isFormCorrect = await this.v$.$validate()
            if (!isFormCorrect) return

            const response = await api
                .post(changePasswordApi, this.params)

            if (response.data) {
                this.onClose();
                messageService.showMessage(response.data.message);
            }
        },
    }
};
</script>