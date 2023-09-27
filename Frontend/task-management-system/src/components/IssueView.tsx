import React, { useEffect, useState } from "react";
import IssuesService from "../Api/IssuesService";
import { useFetching } from "../hooks/useFetching";

function IssueView() {


    return (
        <div className="container">
            <h1>Title</h1>
            <p>Description</p>
        </div>
    );
}

export default IssueView;
