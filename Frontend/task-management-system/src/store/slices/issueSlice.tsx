import { createSlice, PayloadAction } from "@reduxjs/toolkit";
import type { RootState } from "../store";

interface IssueState {
    id: string;
}

const initialState: IssueState = {
    id: "",
};

export const issueSlice = createSlice({
    name: "currentIssue",
    initialState,
    reducers: {
        reset: (state) => {
            state.id = "";
        },
        setId: (state, action: PayloadAction<string>) => {
            state.id = action.payload;
        },
    },
});

// Сгенерированные Создатели Действий/ action creators
export const { reset, setId } = issueSlice.actions;

// Весь остальной код может использовать тип `RootState`
export const selectCurrentIssueId = (state: RootState) => state.currentIssueReducer.id;

export default issueSlice.reducer;
