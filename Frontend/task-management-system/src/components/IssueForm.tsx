import React, { useState } from "react";
import IssuesService from "../Api/IssuesService";
import { useFetching } from "../hooks/useFetching";

function IssueForm() {
    const [id, setItems] = useState<string>();
    const [title, setTitle] = useState("title");
    const [addNewIssue, isLoading, error] = useFetching(async () => {
        const id = await IssuesService.postIssue({ title });
        setItems(id);
    });

    return (
        <div className="container">
            <div className="mb-3">
                <label className="form-label">
                    Issue title
                    <input
                        value={title}
                        onChange={(e) => setTitle(e.target.value)}
                        type="text"
                        className="form-control"
                        placeholder="issue title"
                    />
                </label>
            </div>
            <div className="mb-3">
                <label className="form-label">
                    Example textarea
                    <textarea className="form-control" rows={4}></textarea>
                </label>
            </div>
            <div>
                <button
                    type="submit"
                    className="btn btn-primary"
                    onClick={addNewIssue}>
                    Submit
                </button>
            </div>
        </div>
    );
}

export default IssueForm;
