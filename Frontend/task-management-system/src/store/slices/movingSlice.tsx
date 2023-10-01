import { createSlice, PayloadAction } from "@reduxjs/toolkit";
import type { RootState } from "../store";

interface MovingState {
    ids: string[];
    changedIssuesIds: string[];
}

const initialState: MovingState = {
    ids: [],
    changedIssuesIds: []
};

export const movingSlice = createSlice({
    name: "moving",
    initialState,
    reducers: {
        reset: (state) => {
            state.ids = [];
        },
        register: (state, action: PayloadAction<string>) => {
            state.ids = [...state.ids, action.payload];
        },
        unRegister: (state, action: PayloadAction<string>) => {
            state.ids = state.ids.filter(function (value, index, arr) {
                return value != action.payload;
            });
        },
        registerChangedIssue: (state, action: PayloadAction<string>) => {
            state.changedIssuesIds = [...state.changedIssuesIds, action.payload];
        },
        unregisterChangedIssue: (state, action: PayloadAction<string>) => {
            state.changedIssuesIds = state.changedIssuesIds.filter(function (value, index, arr) {
                return value != action.payload;
            });
        },
    },
});

export const { register, unRegister, registerChangedIssue, unregisterChangedIssue } = movingSlice.actions;

export const movingIssuesId = (state: RootState) => state.movingReducer.ids;
export const changedIssuesId = (state: RootState) => state.movingReducer.changedIssuesIds;

export default movingSlice.reducer;
