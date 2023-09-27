import React, { useState } from "react";
import { ITreeItem } from "../interfaces/ITreeItem";
import TreeGroup from "./TreeGroup";
import axios from "axios";

export function TreeItem({ data }: { data: ITreeItem }) {
    async function fetchChildren() {
        if (!isExpanded) {
            await axios
                .get<ITreeItem[]>(
                    `https://localhost:7081/issues/${data.id}/descendants`
                )
                .then((resp) => {
                    setItems(resp.data);
                });
        } else {
            setItems([]);
        }
        setExpanded(!isExpanded);
    }

    const [isExpanded, setExpanded] = useState(false);
    const [items, setItems] = useState<ITreeItem[]>([]);

    return (
        <div>
            <li className="tree-item">
                <div className="expander">
                    <button hidden={data.isLeaf} onClick={fetchChildren}>
                        V
                    </button>
                </div>
                <div className="tree-item-title">{data.title}</div>
            </li>
            {isExpanded ? <TreeGroup items={items}></TreeGroup> : <></>}
        </div>
    );
}
