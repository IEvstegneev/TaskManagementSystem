import { combineReducers, configureStore } from "@reduxjs/toolkit";
import currentIssueReducer from "./slices/issueSlice";
import counterReducer from "./slices/counterSlice";

const rootReducer = combineReducers({
    counterReducer,
    currentIssueReducer,
});

export const store = configureStore({
    reducer: rootReducer,
});

// Выведение типов `RootState` и `AppDispatch` из хранилища
export type RootState = ReturnType<typeof store.getState>;
// Выведенные типы: {posts: PostsState, comments: CommentsState, users: UsersState}
export type AppDispatch = typeof store.dispatch;

// ++
//export type RootState = ReturnType<typeof rootReducer>
