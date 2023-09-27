import { TreeItem } from "./TreeItem";
import { ITreeItem } from "../interfaces/ITreeItem";

function TreeGroup({ items }: { items: ITreeItem[] }) {
    return (
        <ul className="tree-group">
            {items.map((item) => (
                <TreeItem data={item} key={item.id} />
            ))}
        </ul>
    );
}

export default TreeGroup;
