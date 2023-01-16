import { NavLink } from 'react-router-dom';
import styleDesktop from './FooterDesktop.module.css';
import styleMobile from './FooterMobile.module.css';

function Footer(props) {
    
    // FUNCTIONS *********************************************************************************

    // Render the related footer
    const renderFooter = (isMobile) => {

        if(!isMobile){

            return(
                <footer id={styleDesktop["desktopFooter"]}>
                    <ul>
                        <li>
                            <span>YES!</span>
                        </li>
                        <li>
                            <span>Copyright © 2022</span>
                        </li>
                        <li>
                            <span><NavLink to="/mentions" alt="Mentions légales">Mentions légales</NavLink></span>
                        </li>
                    </ul>
                </footer>
            );

        } else {

            return(
                <footer id={styleMobile["mobileFooter"]}>
                    <span>Copyright © 2022</span>
                    <span><NavLink to="/mentions" alt="Mentions légales">Mentions légales</NavLink></span>
                </footer>
            );

        }

    };

    // RENDERING *********************************************************************************
    
    return (renderFooter(props.screenMobile));
}

export default Footer;
