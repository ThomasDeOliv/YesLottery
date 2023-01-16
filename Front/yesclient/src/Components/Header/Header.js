import { useNavigate, useLocation, NavLink } from 'react-router-dom';
import styleDesktop from './HeaderDesktop.module.css';
import styleMobile from './HeaderMobile.module.css';
import logo from './logo.png';
import menu from './menu.png';
import moneyB from './money_black.png';
import moneyR from './money_red.png';
import play from './play.png';

function Header(props) {

    // HOOKS *************************************************************************************

    // Hook for navigate on the app
    let navigate = useNavigate();

    // Hook to have the current location
    let location = useLocation();

    // FUNCTIONS ********************************************************************************

    // Function to open the mobile menu
    const openMobileMenu = () => {

        // WRITE SOME ACTIONS TO TO 

    };

    // Function rendering the navigation bar relatively of the size of the screen

    const renderHeader = (isMobile) => {

        if(!isMobile){

            return (
                <header id={styleDesktop["desktopHeader"]}>
                    <nav>
                        <ul>
                            <li>
                                <NavLink to="/"><img src={logo} alt="Accueil"/>¥€$!</NavLink>
                            </li>
                            <li>
                                <ul>
                                    <li>
                                        <NavLink to="/" className={({isActive}) => { return isActive ? styleDesktop.activeLink : null }}>Accueil</NavLink>
                                    </li>
                                    <li>
                                        <button type="button" onClick={() => navigate("/play")} disabled={props.currentDrawClosed} className={ location.pathname === "/play" ? styleDesktop.activeLink : null }><img src={play} alt="jouer" /><span>Jouer</span></button>
                                    </li>
                                    <li>
                                        <button type="button" onClick={() => props.setClosedModal(false)} className={ location.pathname === "/result" ? styleDesktop.activeLink : null }>Résultat</button>
                                    </li>
                                    <li>
                                        <NavLink to="/rules" className={({isActive}) => { return isActive ? styleDesktop.activeLink : null }}>Règlement</NavLink>
                                    </li>
                                    <li>
                                        <div id={styleDesktop["headerCounter"]}>
                                            <div>
                                                <span style={props.currentDrawClosed ? {color: '#FA8F89'} : {color: '#000000'}}>
                                                    <span>Prochain jeu : {props.timer}</span> 
                                                    <span> | </span> 
                                                    <span>{props.currentParticipants}</span>
                                                    <img id={styleDesktop["money"]} alt="Cabbage unit symbol" src={props.currentDrawClosed ? moneyR : moneyB}/>
                                                </span>
                                            </div>
                                        </div>
                                    </li>
                                </ul>                
                            </li>
                        </ul>
                    </nav>
                </header>);

        } else {

            return(
                <header id={styleMobile["mobileHeader"]}>
                    <div>
                        <div style={props.currentDrawClosed ? {color: '#FA8F89'} : {color: '#000000'}}>
                            <span>{props.timer}</span>
                            <span>|</span>
                            <span>{props.currentParticipants}</span>
                            <img id={styleMobile["money"]} alt="Cabbage unit symbol" src={props.currentDrawClosed ? moneyR : moneyB}/>
                        </div>
                    </div>
                    <nav id={styleMobile["mobileNav"]}>
                        <button alt="home" onClick={() => navigate("/")}><img src={logo} alt="Accueil"/>¥€$!</button>
                        <button alt="menu" onClick={() => openMobileMenu()}><img src={menu} alt="Burger menu"/></button>               
                    </nav>
                </header>
                );
        }
    };

    // RENDERING *********************************************************************************

    return renderHeader(props.screenMobile);
        
}

export default Header;
