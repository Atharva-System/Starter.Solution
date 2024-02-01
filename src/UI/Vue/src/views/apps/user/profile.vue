<!-- eslint-disable vue/multi-word-component-names -->
<template>
    <div>
        <div class="pt-5">
            <div>
                <form @submit.prevent="submitForm()"
                    class="border border-[#ebedf2] dark:border-[#191e3a] rounded-md p-4 mb-5 bg-white dark:bg-[#0e1726]">
                    <h6 class="text-lg font-bold mb-5">My Profile</h6>
                    <div class="flex flex-col sm:flex-row">
                        <div class="ltr:sm:mr-4 rtl:sm:ml-4 w-full sm:w-2/12 mb-5">
                            <img src="@/assets/images/default_profile.png" alt=""
                                class="w-20 h-20 md:w-32 md:h-32 rounded-full object-cover mx-auto" />
                        </div>
                        <div class="flex-1 grid grid-cols-1 sm:grid-cols-2 gap-5">
                            <div :class="{ 'has-error': v$.params.firstName.$error }">
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
                            <div :class="{ 'has-error': v$.params.lastName.$error }">
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
                            <div>
                                <label for="address">Email</label>
                                <input id="address" type="text" placeholder="New York" class="form-input disabled-textbox"
                                    v-model="params.email" disabled />
                            </div>
                            <div class="sm:col-span-2 mt-3">
                                <button type="submit" class="btn btn-primary" :disabled="isSaveDisabled">Save</button>
                            </div>
                        </div>
                    </div>
                </form>
            </div>
        </div>
    </div>
</template>
<script lang="ts">
import { updateProfile, getProfileDetails } from '@/common/api-paths';
import { noSpaceValidationPattern } from '@/common/constants';
import { useMeta } from '@/composables/use-meta';
import api from '@/services/api';
import messageService from '@/services/message.service';
import tokenService from '@/services/token.service';
import useVuelidate from '@vuelidate/core';
import { required } from '@vuelidate/validators';
import { useStore } from 'vuex'

useMeta({ title: 'Profile Information' });

export default {
    setup() {
        return {
            v$: useVuelidate(),
            $store: useStore()
        }
    },
    data() {
        return {
            isSaveDisabled: false,
            isSubmitForm: false,
            params: { id: '', firstName: '', lastName: '', email: '', },
            originalParams: { id: '', firstName: '', lastName: '', email: '', },
        }
    },
    validations() {
        return {
            params: {
                firstName: { required, noSpace: value => noSpaceValidationPattern.test(value), },
                lastName: { required, noSpace: value => noSpaceValidationPattern.test(value), },
            },
        }
    },
    created() {
        this.setUserDetails();
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

            const response = await api
                .put(updateProfile + this.params.id, this.params)
                ;

            if (response.data) {
                this.originalParams = { ...this.params }
                this.isSaveDisabled = true
                this.$store.commit('updateProfileInfo', {
                    fullName: this.params.firstName + ' ' + this.params.lastName,
                    email: this.params.email,
                })
                tokenService.updateStorageUserInfo(
                    this.params.firstName + ' ' + this.params.lastName,
                    this.params.email,
                );
                messageService.showMessage(response.data.data);
            }
        },
        async setUserDetails() {
            const response = await api
                .get(getProfileDetails);
            if (response.data) {
                this.originalParams = { id: response.data.data.id, firstName: response.data.data.firstName, lastName: response.data.data.lastName, email: response.data.data.email }
                this.params = { id: response.data.data.id, firstName: response.data.data.firstName, lastName: response.data.data.lastName, email: response.data.data.email }
            }
        },
        isParamsUnchanged(newParams) {
            return JSON.stringify(newParams) == JSON.stringify(this.originalParams);
        },
    }
};
</script>
