import { ITreeItem } from "./ITreeItem";
import { IssueStatus } from "./IssueStatus";

export interface IIssue {
    id: string;
    title: string;
    description: string;
    performers: string;
    status: IssueStatus;
    createdAt: string;
    finishedAt?: string;
    estimatedLaborCost: string;
    actualLaborCost: string;
    children?: ITreeItem[];
}


