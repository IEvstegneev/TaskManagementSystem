import React, { useEffect, useState } from "react";
import { ITreeItem } from "../interfaces/ITreeItem";
import TreeGroup from "./TreeGroup";
import { useFetching } from "../hooks/useFetching";
import IssuesService from "../Api/IssuesService";
import { useAppDispatch, useAppSelector } from "../store/hooks";
import { setCurrentId } from "../store/slices/issueSlice";
import {
    movingIssuesId,
    register,
    unRegister,
} from "../store/slices/movingSlice";
import { IIssueItem } from "../interfaces/IIssueItem";

export function IssueItem({ data }: { data: IIssueItem }) {
    const dispatch = useAppDispatch();

    return (
        <button
            type="button"
            className="list-group-item list-group-item-action"
            onClick={() => dispatch(setCurrentId(data.id))}>
            {data.title}
        </button>
    );
}
