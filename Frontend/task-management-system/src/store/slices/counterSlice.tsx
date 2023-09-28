import { createSlice, PayloadAction } from "@reduxjs/toolkit";
import type { RootState } from "../store";

// Определяем тип части состояния(среза/slice)
interface CounterState {
    value: number;
}

// Определение начального состояния, используя тип
const initialState: CounterState = {
    value: 0,
};

export const counterSlice = createSlice({
    name: "counter",
    // `createSlice` выведет тип состояния из аргумента `initialState`
    initialState,
    reducers: {
        increment: (state) => {
            state.value += 1;
        },
        decrement: (state) => {
            state.value -= 1;
        },
        // Использование типа PayloadAction для объявления содержимого `action.payload`
        incrementByAmount: (state, action: PayloadAction<number>) => {
            state.value += action.payload;
        },
    },
});

// Сгенерированные Создатели Действий/ action creators
export const { increment, decrement, incrementByAmount } = counterSlice.actions;

// Весь остальной код может использовать тип `RootState`
export const selectCount = (state: RootState) => state.counterReducer.value;

export default counterSlice.reducer;
