import React, { useEffect } from 'react';
import { Bookmark } from '../../../store/Bookmarks';

const bookmarkComponent = (props: Bookmark) => {
    const imageElement = (props.imageUrl != null ? 
                    <img src={props.imageUrl} alt=''></img> : null);

    return <a href={props.url}>
                {imageElement}
                <p>{props.title}</p>
            </a>
}
export default React.memo(bookmarkComponent);