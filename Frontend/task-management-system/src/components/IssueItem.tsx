import React from "react";
import { useAppDispatch, useAppSelector } from "../store/hooks";
import { setCurrentId } from "../store/slices/issueSlice";
import { IIssueItem } from "../interfaces/IIssueItem";
import { getDisplayStatusName } from "../interfaces/IssueStatus";

export function IssueItem({ data }: { data: IIssueItem }) {
    const dispatch = useAppDispatch();
    return (
        <button
            type="button"
            className="list-group-item list-group-item-action"
            onClick={() => dispatch(setCurrentId(data.id))}>
            <div className="fw-bold">{data.title}</div>
            <div style={{ display: "flex", justifyContent: "space-around" }}>
                <span className="badge bg-warning rounded-pill">
                    {getDisplayStatusName(data.status)}
                </span>
                <span className="badge bg-secondary rounded-pill">
                    план {data.estimatedLaborCost}
                </span>
                <span className="badge bg-primary rounded-pill">
                    факт {data.actualLaborCost}
                </span>
            </div>
        </button>
    );
}
