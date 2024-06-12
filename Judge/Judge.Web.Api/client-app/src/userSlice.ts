import {createSlice, PayloadAction} from '@reduxjs/toolkit'
import {User} from "./api/Api.ts";

export interface UserState {
    user: User | null
}

const initialState: UserState = {
    user: null,
};

export const userSlice = createSlice({
    name: 'user',
    initialState: initialState,
    reducers: {
        setUser: (state, action: PayloadAction<User>) => {
            state.user = action.payload
        },
    },
})

export const {setUser} = userSlice.actions

export default userSlice.reducer