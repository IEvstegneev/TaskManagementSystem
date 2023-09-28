import React, { useState } from "react";
import { ITreeItem } from "../interfaces/ITreeItem";
import TreeGroup from "./TreeGroup";
import { useFetching } from "../hooks/useFetching";
import IssuesService from "../Api/IssuesService";
import { useAppDispatch } from "../store/hooks";
import { setId } from "../store/slices/issueSlice";

export function TreeItem({ data }: { data: ITreeItem }) {
    const dispatch = useAppDispatch();
    const [isExpanded, setExpanded] = useState(false);
    const [items, setItems] = useState<ITreeItem[]>([]);
    const [fetchSubItems, isLoading, error] = useFetching(async () => {
        if (!isExpanded) {
            var subItems = await IssuesService.getIssuesChildrenList(data.id);
            setItems(subItems);
        } else {
            setItems([]);
        }
        setExpanded(!isExpanded);
    });

    return (
        <div>
            <li className="tree-item">
                <div className="expander">
                    <button hidden={data.isLeaf} onClick={fetchSubItems}>
                        V
                    </button>
                </div>
                <div
                    className="tree-item-title"
                    onClick={() => dispatch(setId(data.id))}>
                    {data.title}
                </div>
            </li>
            {isExpanded ? <TreeGroup items={items}></TreeGroup> : <></>}
        </div>
    );
}
