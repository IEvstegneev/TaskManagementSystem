import "../styles/CreateIssueView.css";
import "../styles/IssueForm.css";
import React, { useEffect, useState } from "react";
import IssuesService from "../Api/IssuesService";
import { useFetching } from "../hooks/useFetching";
import { useAppDispatch } from "../store/hooks";
import { setCurrentId } from "../store/slices/issueSlice";

function CreateIssueView({ parentId }: { parentId?: string }) {
    const dispatch = useAppDispatch();
    const [title, setTitle] = useState("");
    const [description, setDescription] = useState("");
    const [performers, setPerformers] = useState("");

    const [addNewIssue, isLoading, error] = useFetching(async () => {
        const id = await IssuesService.postIssue({
            parentId,
            title,
            performers,
            description,
        });
        dispatch(setCurrentId(id));
    });

    return (
        <div className="createIssueView">
            <div className="issueForm">
                <div className="mb-3">
                    <label>
                        Наименование
                        <input
                            type="text"
                            className="title"
                            value={title}
                            onChange={(event) => setTitle(event.target.value)}
                            placeholder="Введите наименование задачи"
                        />
                    </label>
                </div>
                <div className="mb-3">
                    <label>
                        Исполнители
                        <input
                            type="text"
                            className="performers"
                            value={performers}
                            onChange={(event) =>
                                setPerformers(event.target.value)
                            }
                            placeholder="Введите исполнителей задачи"
                        />
                    </label>
                </div>
                <div className="mb-3">
                    <label>
                        Описание
                        <textarea
                            className="description form-control-plaintext"
                            value={description}
                            onChange={(event) =>
                                setDescription(event.target.value)
                            }
                            placeholder="Введите описание задачи, не более 2000 символов."
                            rows={5}
                            maxLength={2000}></textarea>
                    </label>
                </div>
            </div>
            <div className="container">
                {isLoading ? (
                    <>Loading...</>
                ) : (
                    <button
                        type="submit"
                        className="btn btn-success"
                        onClick={addNewIssue}>
                        Создать
                    </button>
                )}
            </div>
        </div>
    );
}

export default CreateIssueView;
