import { createSlice, PayloadAction } from "@reduxjs/toolkit";
import type { RootState } from "../store";

interface IssueState {
    currentId?: string;
    parentId?: string;
}

const initialState: IssueState = {};

export const issueSlice = createSlice({
    name: "currentIssue",
    initialState,
    reducers: {
        resetCurrentId: (state) => {
            state.currentId = undefined;
        },
        resetParentId: (state) => {
            state.parentId = undefined;
        },
        setCurrentId: (state, action: PayloadAction<string>) => {
            state.currentId = action.payload;
        },
        setParentId: (state, action: PayloadAction<string>) => {
            state.parentId = action.payload;
        },
    },
});

export const { resetCurrentId, resetParentId, setCurrentId, setParentId } =
    issueSlice.actions;

export const selectCurrentIssueId = (state: RootState) =>
    state.currentIssueReducer.currentId;
export const selectParentIssueId = (state: RootState) =>
    state.currentIssueReducer.parentId;

export default issueSlice.reducer;
