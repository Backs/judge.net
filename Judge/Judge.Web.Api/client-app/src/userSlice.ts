import {createSlice, PayloadAction} from '@reduxjs/toolkit'
import {CurrentUser} from "./api/Api.ts";

export interface UserState {
    user: CurrentUser | null
}

const initialState: UserState = {
    user: null,
};

export const userSlice = createSlice({
    name: 'user',
    initialState: initialState,
    reducers: {
        setUser: (state, action: PayloadAction<CurrentUser>) => {
            state.user = action.payload
        },
        removeUser: (state) => {
            state.user = null;
        }
    },
})

export const {setUser, removeUser} = userSlice.actions

export default userSlice.reducer