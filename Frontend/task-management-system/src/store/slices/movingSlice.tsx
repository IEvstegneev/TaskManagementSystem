import { createSlice, PayloadAction } from "@reduxjs/toolkit";
import type { RootState } from "../store";

interface MovingState {
    ids: string[];
}

const initialState: MovingState = {
    ids: [],
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
    },
});

export const { register, unRegister } = movingSlice.actions;

export const movingIssuesId = (state: RootState) => state.movingReducer.ids;

export default movingSlice.reducer;