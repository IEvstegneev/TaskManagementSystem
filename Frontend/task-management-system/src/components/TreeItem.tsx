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

export function TreeItem({ data }: { data: ITreeItem }) {
    const dispatch = useAppDispatch();
    const [title, setTitle] = useState<string>(data.title);
    const [isExpanded, setExpanded] = useState(false);
    const [isHidden, setIsHidden] = useState(false);
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

    const canExpand = () => {
        return data.isLeaf;
    };

    //Drag and drop
    const ids = useAppSelector(movingIssuesId);
    useEffect(() => {
        if (ids.find((x) => x === data.id)) setIsHidden(true);
    }, [ids]);

    const dragStartHandler = (
        event: React.DragEvent<HTMLDivElement>,
        data: string
    ) => {
        event.dataTransfer.setData("text", data);
    };

    const allowDrop = (event: React.DragEvent<HTMLDivElement>) => {
        //console.log(event);
        event.preventDefault();
    };

    const dropHandler = async (event: React.DragEvent<HTMLDivElement>) => {
        event.preventDefault();
        const id = event.dataTransfer.getData("text");
        if (id !== data.id) {
            dispatch(register(id));
            await IssuesService.moveIssue(id, data.id);
            dispatch(unRegister(id));
        }
    };

    return (
        <div hidden={isHidden}>
            <li className="tree-item">
                <div className="expander">
                    <button hidden={canExpand()} onClick={fetchSubItems}>
                        V
                    </button>
                </div>
                <div
                    draggable
                    onDragStart={(event) => dragStartHandler(event, data.id)}
                    className="tree-item-title"
                    onClick={() => dispatch(setCurrentId(data.id))}
                    onDragOver={allowDrop}
                    onDrop={dropHandler}>
                    {title}
                </div>
            </li>
            {isExpanded ? <TreeGroup items={items}></TreeGroup> : <></>}
        </div>
    );
}
