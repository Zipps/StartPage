import React, { useEffect } from 'react';

import { Bookmark } from '../../../store/Bookmarks';
import classes from './Bookmark.module.css';

const bookmarkComponent = (props: Bookmark) => {
    const imageElement = props.imageUrl != null ? 
                    <img className={classes.Image} src={props.imageUrl} alt=''></img> : null;

    return <a className={classes.Link} href={props.url}>
                {imageElement}
                <p className={classes.Title}>{props.title}</p>
            </a>
}
export default React.memo(bookmarkComponent);
