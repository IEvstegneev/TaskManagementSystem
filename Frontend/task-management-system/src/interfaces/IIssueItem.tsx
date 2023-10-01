import { IssueStatus } from "./IssueStatus";

export interface IIssueItem {
    id: string;
    title: string;
    status: IssueStatus;
    estimatedLaborCost: string;
    actualLaborCost: string;
}
